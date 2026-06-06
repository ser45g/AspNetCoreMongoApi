namespace AspNetCoreMongoApi.Options
{
    public class WeatherForecastValidationOptions
    {
        public static readonly string ConfigurationSection = nameof(WeatherForecastValidationOptions);
        public int MinSummaryLength { get; init; } = 0;
        public int MaxSummaryLength { get; init; } = 200;
        public int MinTemperatureC { get; init; } = -273;
        public int MaxTemperatureC { get; init; } = 1000;
    }
}
