namespace AspNetCoreMongoApi.Contracts.Response
{
 
    public record class WeatherForecastDto(Guid Id, DateOnly Date, int TemperatureC, int TemperatureF, string? Summary);

}
