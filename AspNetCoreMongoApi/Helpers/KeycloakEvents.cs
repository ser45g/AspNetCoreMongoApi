namespace AspNetCoreMongoApi.Helpers
{
    public static class KeycloakEvents
    {

        public static string AccountDeletedAdmin(string realmName) => $"KK.EVENT.ADMIN.{realmName}.SUCCESS.USER.DELETE";
        public static string AccountDeletedClient(string realmName) => $"KK.EVENT.CLIENT.{realmName}.SUCCESS.account-console.DELETE_ACCOUNT";

        public static string LoginClient(string realmName, string clientId) => $"KK.EVENT.CLIENT.{realmName}.SUCCESS.{clientId}.LOGIN";
        public static string AllEvents() => "KK.EVENT.#";

        public static string AllRealmEvents(string realmName) => $"KK.EVENT.*.{realmName}.#";

        public static string AccountCreatedAdmin(string realmName) => $"KK.EVENT.ADMIN.{realmName}.SUCCESS.USER.CREATE";

        public static string EmailVerifiedClient(string realmName, string clientId) => $"KK.EVENT.CLIENT.{realmName}.SUCCESS.{clientId}.VERIFY_EMAIL";

        public static string SendVerifyEmailClient(string realmName, string clientId) => $"KK.EVENT.CLIENT.{realmName}.SUCCESS.{clientId}.SEND_VERIFY_EMAIL";

        
    }
}
