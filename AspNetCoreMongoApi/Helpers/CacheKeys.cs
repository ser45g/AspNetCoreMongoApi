namespace AspNetCoreMongoApi.Helpers
{
    public static class CacheKeys
    {
        public static string WeatherForecastById(Guid id) => $"weather-forecast-{id}";

        public static string TodoById(Guid id) => $"todo-{id}";
    }
}
