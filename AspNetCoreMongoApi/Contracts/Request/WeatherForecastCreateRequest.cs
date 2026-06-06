namespace AspNetCoreMongoApi.Contracts.Request
{
    public record class WeatherForecastCreateRequest(DateOnly? Date, int? TemperatureC, string? Summary);
}
