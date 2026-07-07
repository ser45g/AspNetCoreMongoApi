namespace AspNetCoreMongoApi.Consumers.Keycloak.UserDeleted.Events
{
    public record EventClientUserDeleted(
        string @Class,
        long Time,
        string Type,
        string RealmId,
        string ClientId,
        string UserId,
        string IpAddress,
        EventClientUserDeletedDetails Details
);
}
