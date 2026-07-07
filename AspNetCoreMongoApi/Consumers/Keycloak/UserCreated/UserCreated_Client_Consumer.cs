
using AspNetCoreMongoApi.Consumers.Keycloak.UserCreated.Events;
using AspNetCoreMongoApi.Services;
using MassTransit;
using System.Text.Json;

namespace AspNetCoreMongoApi.Consumers.Keycloak.UserCreated
{
    public class UserCreated_Client_Consumer(IEmailService emailService) : IConsumer<EventClientUserEmailVerified>
    {       
        public async Task Consume(ConsumeContext<EventClientUserEmailVerified> context)
        {
            var email = context.Message?.Details.Email;

            if (string.IsNullOrWhiteSpace(email))
            {
                throw new Exception("Could not process the user created event.");
            }

            await emailService.SendEmailAsync(email, "Account Created", "Your account was created. Thank you for joining in!", context.CancellationToken);
        }
    }
}
