namespace AspNetCoreMongoApi.Consumers.Keycloak.UserCreated.Events
{
    public record class EventAdminUserCreatedAuthDetails(
        string RealmId,
        string RealmName,
        string ClientId,
        string UserId,
        string IpAddress
    );
}
