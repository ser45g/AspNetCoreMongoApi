namespace AspNetCoreMongoApi.Contracts.Request
{
    public record class CreateWeatherForecastRequest(DateOnly? Date, int? TemperatureC, string? Summary);
}
