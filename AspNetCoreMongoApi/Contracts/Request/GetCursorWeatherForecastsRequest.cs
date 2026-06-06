namespace AspNetCoreMongoApi.Contracts.Request
{
    public record class GetCursorWeatherForecastsRequest(int? PageSize=null, Guid? Cursor=null);
}
