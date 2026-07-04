using System.Text.Json.Serialization;

namespace AspNetCoreMongoApi.Consumers.Keycloak.Events
{
    public record class KeycloakAdminEventMessage(
        [property: JsonPropertyName("@class")] string Class,
        long Time,
        string RealmId,
        KeycloakAdminEventAuthDetails AuthDetails,
        string ResourceType,
        string OperationType,
        string ResourcePath,
        string Representation,
        string ResourceTypeAsString,
        string ResourceId);
}

