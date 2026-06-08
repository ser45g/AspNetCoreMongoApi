using System.ComponentModel.DataAnnotations;

namespace AspNetCoreMongoApi.Options
{
    public class CorsOptions
    {
        public static readonly string ConfigurationSection = nameof(CorsOptions);

        public required IEnumerable<string> AllowedOrigins { get; set; }
    }
}
