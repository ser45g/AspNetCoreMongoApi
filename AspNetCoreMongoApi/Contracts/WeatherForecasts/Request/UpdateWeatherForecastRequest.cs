namespace AspNetCoreMongoApi.Contracts.WeatherForecasts.Request
{
    public record class UpdateWeatherForecastRequest(Guid Id, DateOnly? Date, int? TemperatureC, string? Summary);
}
