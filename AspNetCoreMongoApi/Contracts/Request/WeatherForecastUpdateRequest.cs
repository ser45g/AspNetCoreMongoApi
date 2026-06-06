namespace AspNetCoreMongoApi.Contracts.Request
{
    public record class WeatherForecastUpdateRequest(Guid Id, DateOnly? Date, int? TemperatureC, string? Summary);
}
