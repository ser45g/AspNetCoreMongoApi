using AspNetCoreMongoApi.Consumers.Cleanup;
using AspNetCoreMongoApi.Consumers.Keycloak.UserDeleted.Events;
using AspNetCoreMongoApi.Helpers;
using MassTransit;

namespace AspNetCoreMongoApi.Consumers.Keycloak.UserDeleted
{
    public class UserDeleted_Cleanup_Admin_Consumer(IPublishEndpoint publishEndpoint) : IConsumer<EventAdminUserDeleted>
    {       
        public async Task Consume(ConsumeContext<EventAdminUserDeleted> context)
        {

            var userId = context.Message.ResourceId;

            if (string.IsNullOrEmpty(userId))
            {
                return;
            }
            await publishEndpoint.Publish(new CleanupByUserIdEvent(userId),ctx=>{
                ctx.SetRoutingKey(AppEvents.CleanupByUserIdRoutingKey);
            }, cancellationToken: context.CancellationToken);
        }
    }
}
