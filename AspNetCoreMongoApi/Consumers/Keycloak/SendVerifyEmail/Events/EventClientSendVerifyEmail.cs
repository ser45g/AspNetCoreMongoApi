namespace AspNetCoreMongoApi.Consumers.Keycloak.SendVerifyEmail.Events
{
    public record class EventClientSendVerifyEmail(
        string Class,
        long Time,
        string Type,
        string RealmId,
        string ClientId,
        string UserId,
        string IpAddress,
        EventClientSendVerifyEmailDetails Details
    );
}
