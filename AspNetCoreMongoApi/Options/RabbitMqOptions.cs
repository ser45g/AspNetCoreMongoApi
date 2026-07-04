namespace AspNetCoreMongoApi.Options
{
    public class RabbitMqOptions
    {
        public static readonly string ConfigurationSection = nameof(RabbitMqOptions);
        public required string Url { get; init; }
        public required string Username { get; init; }
        public required string Password { get; init; }
        public bool IsSslUsed { get; init; } = true;
    }
}
