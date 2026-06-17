namespace AspNetCoreMongoApi.Options
{
    public class ElasticSearchOptions
    {
        public static readonly string ConfigurationSection = nameof(ElasticSearchOptions);
        public required string Url { get; init; }
        public string? CloudApiKey { get; init; }
        public string? LocalUsername { get; init; }
        public string? LocalPassword { get; init; }
        public bool IsSslUsed { get; init; } = false;
    }
}
