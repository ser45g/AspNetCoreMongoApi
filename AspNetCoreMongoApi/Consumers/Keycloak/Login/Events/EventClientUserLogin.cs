namespace AspNetCoreMongoApi.Consumers.Keycloak.Login.Events
{
   
    public record class EventClientUserLogin(
        string Class,
        long Time,
        string Type,
        string RealmId,
        string ClientId,
        string UserId,
        string IpAddress,
        EventClientUserLoginDetails Details
    );
}
