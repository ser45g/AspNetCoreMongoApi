namespace AspNetCoreMongoApi.Contracts
{
    public record class WeatherForecastUpdateDto(Guid Id, DateOnly Date, int TemperatureC, string? Summary);
}
