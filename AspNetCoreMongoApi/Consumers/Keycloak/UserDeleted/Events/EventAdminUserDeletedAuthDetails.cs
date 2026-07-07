namespace AspNetCoreMongoApi.Consumers.Keycloak.UserDeleted.Events
{
    public record EventAdminUserDeletedAuthDetails(
        string RealmId,
        string RealmName,
        string ClientId,
        string UserId,
        string IpAddress
    );
}
