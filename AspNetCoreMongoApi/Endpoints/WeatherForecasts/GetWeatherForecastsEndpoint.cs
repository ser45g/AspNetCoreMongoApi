using AspNetCoreMongoApi.Contracts.Common.Response;
using AspNetCoreMongoApi.Contracts.WeatherForecasts.Request;
using AspNetCoreMongoApi.Contracts.WeatherForecasts.Response;
using AspNetCoreMongoApi.Data;
using AspNetCoreMongoApi.Entities;
using AspNetCoreMongoApi.Extensions;
using AspNetCoreMongoApi.Helpers;
using AspNetCoreMongoApi.Options;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;

namespace AspNetCoreMongoApi.Endpoints.WeatherForecasts
{
    public class GetWeatherForecastsEndpoint(AutoMapper.IMapper mapper, MongoDbContext dbContext, IOptions<PaginationOptions> paginationOptions) : Endpoint<GetCursorWeatherForecastsRequest, CursorPaginationResponse<IEnumerable<WeatherForecastResponse>>>
    {
        public override void Configure()
        {
            Get(EndpointRoutes.WeatherForecast);
        }

        public override async Task HandleAsync(GetCursorWeatherForecastsRequest request, CancellationToken ct)
        {
            var startWith = request.Cursor ?? Guid.Empty;
            var pageSize = request.PageSize ?? paginationOptions.Value.DefaultPageSize;

            IQueryable<WeatherForecast> weatherForecastsQuery = dbContext.WeatherForecasts.AsNoTracking();

            weatherForecastsQuery = weatherForecastsQuery.Where(w => w.Id >= startWith);

            weatherForecastsQuery= weatherForecastsQuery.AddFilters(request.MinDate, request.MaxDate, request.MinTemperatureC, request.MaxTemperatureC, request.SummarySearchTerm);

            var keySelector = GetKeySelector(request.SortColumn);

            weatherForecastsQuery = request.SortAsc==true ? weatherForecastsQuery.OrderBy(keySelector): weatherForecastsQuery.OrderByDescending(keySelector);

            int totalCount = await weatherForecastsQuery.CountAsync(); 

            var weatherForecasts = await weatherForecastsQuery.Take(pageSize + 1).ToListAsync(ct);

            Guid? cursor = null;

            if(weatherForecasts.Count == pageSize + 1)
            {
                var last = weatherForecasts.LastOrDefault();

                if (last != null)
                {
                    cursor = last.Id;
                    weatherForecasts.Remove(last);
                }
            }

            var weatherForecastDtos = mapper.Map<IEnumerable<WeatherForecastResponse>>(weatherForecasts);

            var response = new CursorPaginationResponse<IEnumerable<WeatherForecastResponse>>(cursor, weatherForecastDtos, weatherForecasts.Count, totalCount);

            await Send.OkAsync(response, ct);
        }

        private Expression<Func<WeatherForecast, object>> GetKeySelector(string? sortColumn=null)
        {
            Expression<Func<WeatherForecast, object>> keySelector = sortColumn?.ToLower() switch
            {
                "temperaturec" => w => w.TemperatureC,
                "date" => w => w.Date,
                _ => w => w.Id
            };
            return keySelector;
        }

    }
}
