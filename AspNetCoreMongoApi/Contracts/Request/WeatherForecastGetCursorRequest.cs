namespace AspNetCoreMongoApi.Contracts.Request
{
    public record class WeatherForecastGetCursorRequest(int PageSize=10, Guid? Cursor=null);
}
