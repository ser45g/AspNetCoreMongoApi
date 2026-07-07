using AspNetCoreMongoApi.Consumers.Cleanup;
using AspNetCoreMongoApi.Consumers.Keycloak.UserDeleted.Events;
using AspNetCoreMongoApi.Helpers;
using MassTransit;

namespace AspNetCoreMongoApi.Consumers.Keycloak.UserDeleted
{
    public class UserDeleted_Cleanup_Client_Consumer(IPublishEndpoint publishEndpoint) : IConsumer<EventClientUserDeleted>
    {       
        public async Task Consume(ConsumeContext<EventClientUserDeleted> context)
        {
            var message = context.Message;

            var userId = message?.UserId;

            if (userId == null)
            {
                return;
            }

            await publishEndpoint.Publish(new CleanupByUserIdEvent(userId), ctx => {
                ctx.SetRoutingKey(AppEvents.CleanupByUserIdRoutingKey);
            }, cancellationToken: context.CancellationToken);
        }
    }
}
