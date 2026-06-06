namespace AspNetCoreMongoApi.Contracts.Request
{
    public record class UpdateWeatherForecastRequest(Guid Id, DateOnly? Date, int? TemperatureC, string? Summary);
}
