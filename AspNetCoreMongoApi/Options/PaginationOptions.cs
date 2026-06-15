namespace AspNetCoreMongoApi.Options
{
    public class PaginationOptions
    {
        public static readonly string ConfigurationSection = nameof(PaginationOptions);
        public int DefaultPageSize { get; init; } = 10;
        public int MinPageSize { get; init; } = 1;
        public int MaxPageSize { get; init; } = 100;

    }
}
