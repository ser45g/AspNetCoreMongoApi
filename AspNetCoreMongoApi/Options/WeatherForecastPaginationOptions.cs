namespace AspNetCoreMongoApi.Options
{
    public class WeatherForecastPaginationOptions
    {
        public static readonly string ConfigurationSection = nameof(WeatherForecastPaginationOptions);
        public int DefaultPageSize { get; init; } = 10;
        public int MinPageSize { get; init; } = 1;
        public int MaxPageSize { get; init; } = 100;

    }
}
