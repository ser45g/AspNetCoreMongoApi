namespace AspNetCoreMongoApi.Consumers.Keycloak.UserCreated.Events
{
    public record class EventAdminUserCreated(
        string @Class,
        long Time,
        string RealmId,
        EventAdminUserCreatedAuthDetails AuthDetails,
        string ResourceType,
        string OperationType,
        string ResourcePath,
        string Representation,
        string ResourceTypeAsString,
        string ResourceId
    );
}
