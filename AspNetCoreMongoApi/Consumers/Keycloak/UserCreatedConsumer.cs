using AspNetCoreMongoApi.Consumers.Keycloak.Events;
using MassTransit;
using System.Text.Json;

namespace AspNetCoreMongoApi.Consumers.Keycloak
{
    public class UserCreatedConsumer(ILogger<UserCreatedConsumer> _logger) : IConsumer<KeycloakAdminEventMessage>
    {       
        public async Task Consume(ConsumeContext<KeycloakAdminEventMessage> context)
        {
            var @event = context.Message;
            
            var user = JsonSerializer.Deserialize<UserRepresentation>(@event.Representation, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true // This handles case mismatches
            });
            
            _logger.LogInformation("Hello");

        }
    }
}
