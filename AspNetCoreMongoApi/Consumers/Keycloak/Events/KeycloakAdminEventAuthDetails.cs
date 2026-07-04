namespace AspNetCoreMongoApi.Consumers.Keycloak.Events
{
    public record KeycloakAdminEventAuthDetails(string RealmId, string RealmName, string ClientId, string UserId, string IpAddress);
}

