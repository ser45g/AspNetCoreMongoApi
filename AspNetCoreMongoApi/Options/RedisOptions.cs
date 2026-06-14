namespace AspNetCoreMongoApi.Options
{
    public class RedisOptions
    {
        public static readonly string ConfigurationSection = nameof(RedisOptions);

        public required string Configuration { get; init; } 

        public required string Password { get; init; }
    }
}

