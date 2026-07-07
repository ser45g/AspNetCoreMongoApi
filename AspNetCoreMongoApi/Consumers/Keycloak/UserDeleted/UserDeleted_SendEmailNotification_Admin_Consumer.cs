using AspNetCoreMongoApi.Consumers.Keycloak.UserDeleted.Events;
using AspNetCoreMongoApi.Services;
using MassTransit;
using System.Text.Json;

namespace AspNetCoreMongoApi.Consumers.Keycloak.UserDeleted
{
    public class UserDeleted_SendEmailNotification_Admin_Consumer(IEmailService emailService) : IConsumer<EventAdminUserDeleted>
    {
        public async Task Consume(ConsumeContext<EventAdminUserDeleted> context)
        {
            var @event = context.Message;

            var user = JsonSerializer.Deserialize<EventAdminUserDeletedUserRepresentation>(@event.Representation, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true // This handles case mismatches
            });

            if (user == null)
            {
                throw new Exception("Could not process the user deleted event.");
            }

            if (user.Username==null)
            {
                return;
            }

            await emailService.SendEmailAsync(user.Username, "Account Deleted", "Your account was deleted", context.CancellationToken);
        }
    }
}
