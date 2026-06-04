namespace AspNetCoreMongoApi.Contracts.Response
{
    public record class WeatherForecastCursorResponse<T>(Guid? NextCursor, T Data, int Count);
}
