
namespace AspNetCoreMongoApi.Options
{
    public class MailOptions
    {
        public static readonly string ConfigurationSection = nameof(MailOptions);

        public required string Host { get; init; }

        public required string From { get; init; }

        public required string DisplayName { get; init; }

        public string? UserName { get; init; }

        public string? Password { get; init; }

        public int Port { get; init; } = 2525;

        public bool IsAuthenticated { get; init; } = true;
    }
}
