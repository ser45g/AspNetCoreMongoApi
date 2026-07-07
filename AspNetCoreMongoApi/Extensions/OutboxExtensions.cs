using AspNetCoreMongoApi.Data;
using AspNetCoreMongoApi.Outbox;
using System.Text.Json;

namespace AspNetCoreMongoApi.Extensions
{
    public static class OutboxExtensions
    {
        public static async Task InsertOutboxMessage<T>(this AppDbContext dbContext, T message, CancellationToken cancellationToken = default) where T : notnull {

            ArgumentNullException.ThrowIfNull(message);

            var outboxMessage = new OutboxMessage(Guid.NewGuid(), message.GetType().FullName!, JsonSerializer.Serialize<T>(message), DateTime.UtcNow);
           
            dbContext.OutboxMessages.Add(outboxMessage);

            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public static void InsertOutboxMessageWithoutSaving<T>(this AppDbContext dbContext, T message, CancellationToken cancellationToken = default) where T : notnull {

            ArgumentNullException.ThrowIfNull(message);

            var outboxMessage = new OutboxMessage(Guid.NewGuid(), message.GetType().FullName!, JsonSerializer.Serialize<T>(message), DateTime.UtcNow);

            dbContext.OutboxMessages.Add(outboxMessage);

        }
    }
}
