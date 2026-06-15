namespace AspNetCoreMongoApi.Contracts.WeatherForecasts.Request
{
    public record class CreateWeatherForecastRequest(DateOnly? Date, int? TemperatureC, string? Summary);
}
