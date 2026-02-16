
namespace AspNetCoreMongoApi.Options
{
    public class SeqOptions
    {
        public static readonly string ConfigurationSection = nameof(SeqOptions);

        public string OtlpEndpoint { get {
                return $@"{Url}/ingest/otlp/v1/traces";
            } 
        }

        public required string Url { get; init; }
    }
}
