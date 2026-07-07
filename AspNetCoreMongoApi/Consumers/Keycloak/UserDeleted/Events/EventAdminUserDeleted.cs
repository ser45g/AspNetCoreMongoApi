namespace AspNetCoreMongoApi.Consumers.Keycloak.UserDeleted.Events
{
    public record EventAdminUserDeleted(
        string @Class,
        long Time,
        string RealmId,
        EventAdminUserDeletedAuthDetails AuthDetails,
        string ResourceType,
        string OperationType,
        string ResourcePath,
        string Representation,
        string ResourceTypeAsString,
        string ResourceId
    );
}
