namespace AspNetCoreMongoApi.Helpers
{
    public static class AppEvents
    {
        public const string CleanupByUserIdRoutingKey = "app.cleanup-by-user-id";

        public const string CleanupByUserIdExchange = "app-exchange";

        public const string CleanupByUserIdQueue = $"{CleanupByUserIdRoutingKey}-queue";

        public const string KeycloakEventsExchange = "keycloak-events";


    }
}
