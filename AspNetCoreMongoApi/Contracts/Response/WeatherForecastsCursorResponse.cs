namespace AspNetCoreMongoApi.Contracts.Response
{
    public record class WeatherForecastsCursorResponse<T>(Guid? NextCursor, T Data, int Count);
}
