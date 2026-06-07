using AspNetCoreMongoApi.Entities;

namespace AspNetCoreMongoApi.Extensions
{
    public static class QueryableFilteringExtensions
    {
        //IQueryable is immutable - methods like Where() don't modify the original query. They return a new IQueryable with the filter applied.
        public static IQueryable<WeatherForecast> AddFilters(this IQueryable<WeatherForecast> weatherForecastsQuery, DateOnly? minDate = null, DateOnly? maxDate = null, int? minTemperatureC = null, int? maxTemperatureC = null, string? summarySearchTerm = null)
        {
            if (maxTemperatureC != null)
            {
                weatherForecastsQuery = weatherForecastsQuery.Where(w => w.TemperatureC <= maxTemperatureC);
            }

            if (minTemperatureC != null)
            {
                weatherForecastsQuery = weatherForecastsQuery.Where(w => w.TemperatureC >= minTemperatureC);
            }

            if (minDate != null)
            {
                weatherForecastsQuery = weatherForecastsQuery.Where(w => w.Date >= minDate);
            }

            if (maxDate != null)
            {
                weatherForecastsQuery = weatherForecastsQuery.Where(w => w.Date <= maxDate);
            }

            if (summarySearchTerm != null)
            {
                weatherForecastsQuery = weatherForecastsQuery.Where(w => w.Summary != null && w.Summary.Contains(summarySearchTerm, StringComparison.CurrentCultureIgnoreCase));
            }

            return weatherForecastsQuery;
        }
    }
}
