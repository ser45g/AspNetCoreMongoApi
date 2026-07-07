using AspNetCoreMongoApi.Options;
using Microsoft.Extensions.Options;

namespace AspNetCoreMongoApi.Outbox
{
    public class OutboxBackgroundService(IServiceScopeFactory scopeFactory, IOptions<OutboxBackgroundServiceOptions> options) : BackgroundService
    {
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var intervalMiliseconds = options.Value.IntervalMiliseconds;
            var maxDegreeOfParallelism = options.Value.MaxDegreeOfParallelism;

            var parallelOptions = new ParallelOptions() { CancellationToken = stoppingToken, MaxDegreeOfParallelism = maxDegreeOfParallelism };

            await Parallel.ForEachAsync(Enumerable.Range(0, options.Value.MaxDegreeOfParallelism), parallelOptions, async (index, ct) =>
            {
                await ProcessOutboxMessages(stoppingToken, intervalMiliseconds);
            }); 
        }

        private async Task ProcessOutboxMessages(CancellationToken stoppingToken, int intervalMiliseconds)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = scopeFactory.CreateScope();

                var outboxProcessor = scope.ServiceProvider.GetRequiredService<OutboxProcessor>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<OutboxBackgroundService>>();

                try
                {
                    await outboxProcessor.ProcessOutboxMessagesAsync(stoppingToken);
                }
                catch (Exception ex) {
                    logger.LogError($"Couldn't process outbox messages. The error was: {ex.Message}");
                }

                await Task.Delay(intervalMiliseconds, stoppingToken);
            }
        }
    }
}
