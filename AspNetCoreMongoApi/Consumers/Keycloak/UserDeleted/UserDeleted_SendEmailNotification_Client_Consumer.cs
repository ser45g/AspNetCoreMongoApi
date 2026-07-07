using AspNetCoreMongoApi.Consumers.Keycloak.UserDeleted.Events;
using AspNetCoreMongoApi.Services;
using MassTransit;

namespace AspNetCoreMongoApi.Consumers.Keycloak.UserDeleted
{
    public class UserDeleted_SendEmailNotification_Client_Consumer(IEmailService emailService) : IConsumer<EventClientUserDeleted>
    {
        public async Task Consume(ConsumeContext<EventClientUserDeleted> context)
        {
            var email = context.Message.Details.Username;

            if(email == null)
            {
                return;
            }

            await emailService.SendEmailAsync(email, "Account Deleted", "Your account was deleted", context.CancellationToken);
        }
    }
}
