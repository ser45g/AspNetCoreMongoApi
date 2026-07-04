using AspNetCoreMongoApi.Consumers.Keycloak.Events;
using AspNetCoreMongoApi.Services;
using MassTransit;
using System.Text.Json;

namespace AspNetCoreMongoApi.Consumers.Keycloak
{
    public class UserCreatedConsumer(ILogger<UserCreatedConsumer> logger, IEmailService emailService) : IConsumer<KeycloakAdminEventMessage>
    {       
        public async Task Consume(ConsumeContext<KeycloakAdminEventMessage> context)
        {
            var @event = context.Message;
            try
            {
                var user = JsonSerializer.Deserialize<UserRepresentation>(@event.Representation, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true // This handles case mismatches
                });
                if (user == null) {
                    throw new Exception("Could not process the user created event.");
                }

                await emailService.SendEmailAsync(user.Email, "Account Created", "Your account was created. Thank you for joining in!");
            }
            catch (Exception ex) {
                throw new Exception("Could not process the user created event.");
            }

        }
    }
}
