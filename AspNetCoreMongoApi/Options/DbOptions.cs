namespace AspNetCoreMongoApi.Options
{
    public class DbOptions
    {
        public static readonly string ConfigurationSection = nameof(DbOptions);

        public required string ConnectionString { get; init; }

    }
}
