namespace AspNetCoreMongoApi.Contracts.Request
{
    public record class WeatherForecastGetCursorRequest(int? PageSize=null, Guid? Cursor=null);
}
