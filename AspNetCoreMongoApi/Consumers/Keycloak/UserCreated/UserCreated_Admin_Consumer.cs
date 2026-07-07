
using AspNetCoreMongoApi.Consumers.Keycloak.UserCreated.Events;
using AspNetCoreMongoApi.Services;
using MassTransit;
using System.Text.Json;

namespace AspNetCoreMongoApi.Consumers.Keycloak.UserCreated
{
    public class UserCreated_Admin_Consumer(IEmailService emailService) : IConsumer<EventAdminUserCreated>
    {       
        public async Task Consume(ConsumeContext<EventAdminUserCreated> context)
        {
            var @event = context.Message;

            var user = JsonSerializer.Deserialize<EventAdminUserCreatedUserRepresentation>(@event.Representation, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true // This handles case mismatches
            });

            if (user==null)
            {
                throw new Exception("Could not process the user created event.");
            }

            if (!user.EmailVerified || user.Email == null)
            {
                return;
            }

            await emailService.SendEmailAsync(user.Email, "Account Created", "Your account was created. Thank you for joining in!", context.CancellationToken);
        }
    }
}
