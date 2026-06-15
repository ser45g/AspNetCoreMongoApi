namespace AspNetCoreMongoApi.Contracts.WeatherForecasts.Request
{
    public record class GetCursorWeatherForecastsRequest(
        DateOnly? MinDate=null,
        DateOnly? MaxDate=null,

        int? MinTemperatureC=null,
        int? MaxTemperatureC=null,

        string? SummarySearchTerm=null,

        string? SortColumn=null,
        bool? SortAsc = true,

        int? PageSize=null,
        Guid? Cursor=null);
}
