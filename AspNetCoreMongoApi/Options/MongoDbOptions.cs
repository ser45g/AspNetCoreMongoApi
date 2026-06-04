namespace AspNetCoreMongoApi.Options
{
    public class MongoDbOptions
    {
        public static readonly string ConfigurationSection = nameof(MongoDbOptions);

        public string ConnectionString { get {
                return $@"mongodb://{Username}:{Password}@{Url}/db?authMechanism=SCRAM-SHA-256&authSource=admin";
            }
        }
       
        public required string Url { get; init; }

        public required string Username { get; init; }

        public required string Password { get; init; }
    }
}
