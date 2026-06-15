namespace AspNetCoreMongoApi.Contracts.WeatherForecasts.Response
{
    public record class WeatherForecastResponse(Guid Id, DateOnly Date, int TemperatureC, int TemperatureF, string? Summary);
}
