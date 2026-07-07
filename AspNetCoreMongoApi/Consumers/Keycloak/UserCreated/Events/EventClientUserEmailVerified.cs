namespace AspNetCoreMongoApi.Consumers.Keycloak.UserCreated.Events
{
    public record class EventClientUserEmailVerified(
        string @Class,
        long Time,
        string Type,
        string RealmId,
        string ClientId,
        string UserId,
        string IpAddress,
        EventClientUserEmailVerifiedDetails Details
    );
}
