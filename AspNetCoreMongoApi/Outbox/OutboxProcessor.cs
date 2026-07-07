using AspNetCoreMongoApi.Data;
using AspNetCoreMongoApi.Options;
using EFCore.BulkExtensions;
using EFCore.PostgresExtensions.Enums;
using EFCore.PostgresExtensions.Extensions;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using System.Collections.Concurrent;
using System.Reflection;
using System.Text.Json;

namespace AspNetCoreMongoApi.Outbox
{
    public partial class OutboxProcessor(AppDbContext dbContext, IPublishEndpoint sender, IOptions<OutboxBackgroundServiceOptions> options)
    {
        private static readonly ConcurrentDictionary<string, Type> TypeCache = new();

        private static readonly AsyncRetryPolicy RetryPolicy = Policy.Handle<Exception>().WaitAndRetryAsync(3, t=> TimeSpan.FromMilliseconds(t*150));

        private static Type? GetOrAddMessageType(string typeName, Assembly assembly)
        {
            var type = assembly.GetType(typeName);

            return type != null ? TypeCache.GetOrAdd(typeName, type) : null ;
        }

        public async Task<int> ProcessOutboxMessagesAsync(CancellationToken stoppingToken=default)
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync(stoppingToken);

            List<OutboxMessage> nonProcessedMessages= await dbContext.OutboxMessages.AsNoTracking().Where(m => m.ProcessedOnUtc == null).OrderBy(m=>m.OccuredOnUtc).Take(options.Value.BatchSize).ForUpdate<OutboxMessage>(LockBehavior.SkipLocked).ToListAsync(stoppingToken);
            
            if (nonProcessedMessages.Count == 0) {
                return 0;
            }
            var updateQueue = new ConcurrentQueue<OutboxMessage>();

            var assembly = Assembly.GetExecutingAssembly();

            nonProcessedMessages.ForEach(x => EnqueueMessage(x, updateQueue, assembly));

            var entities = updateQueue.ToList();

            await RetryPolicy.ExecuteAsync(async () =>
                await sender.PublishBatch<OutboxMessage>(entities, cancellationToken: stoppingToken));

            await RetryPolicy.ExecuteAsync(async () => 
                await dbContext.BulkUpdateAsync(entities, new BulkConfig() { PropertiesToInclude = new List<string> { nameof(OutboxMessage.ProcessedOnUtc), nameof(OutboxMessage.Error)}}, cancellationToken:stoppingToken));

            await transaction.CommitAsync(stoppingToken);

            return nonProcessedMessages.Count;
        }

        public static void EnqueueMessage(OutboxMessage message, ConcurrentQueue<OutboxMessage> updateQueue, Assembly assembly) {
            try
            {
                Type? msgType = GetOrAddMessageType(message.Type, assembly);

                var deserializedMessage = JsonSerializer.Deserialize(message.Content, msgType) ?? throw new Exception("Could not deserialize the message");
                
                updateQueue.Enqueue(message with { ProcessedOnUtc=DateTime.UtcNow });
            }
            catch (Exception ex)
            {
                updateQueue.Enqueue(message with { ProcessedOnUtc = null, Error = ex.ToString() });
            }
        }
        
    }
}
