using MassTransit;
using System.Text;

namespace AspNetCoreMongoApi.Consumers.Keycloak
{
    public record class Msg();
    public class AllCatchConsumer(ILogger<AllCatchConsumer> logger) : IConsumer<Msg>
    {
        public Task Consume(ConsumeContext<Msg> context)
        {
            logger.LogInformation($"Routing key: {context.RoutingKey()}" );

            var body = context.ReceiveContext.GetBody();
            var rawJson = Encoding.UTF8.GetString(body);
            logger.LogInformation("Raw JSON payload: {Json}", rawJson);

            return Task.CompletedTask;
        }
    }
}
