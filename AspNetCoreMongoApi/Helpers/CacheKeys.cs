namespace AspNetCoreMongoApi.Helpers
{
    public static class CacheKeys
    {
        public static string WeatherForecastById(Guid id) => $"weather-forecast-{id}";

        internal static string TodoById(object id)
        {
            throw new NotImplementedException();
        }
    }
}
