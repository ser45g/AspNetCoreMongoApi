namespace AspNetCoreMongoApi.Contracts
{
    public record class WeatherForecastCreateDto(DateOnly Date, int TemperatureC, string? Summary);
}
