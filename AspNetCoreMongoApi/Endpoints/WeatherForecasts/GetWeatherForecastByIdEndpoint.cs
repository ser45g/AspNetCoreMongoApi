using AspNetCoreMongoApi.Contracts.WeatherForecasts.Request;
using AspNetCoreMongoApi.Contracts.WeatherForecasts.Response;
using AspNetCoreMongoApi.Data;
using AspNetCoreMongoApi.Helpers;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using ZiggyCreatures.Caching.Fusion;
using IMapper = AutoMapper.IMapper;

namespace AspNetCoreMongoApi.Endpoints.WeatherForecasts
{
    public class GetWeatherForecastByIdEndpoint(IMapper mapper, MongoDbContext context, IFusionCache hybridCache):Endpoint<GetByIdWeatherForecastRequest, WeatherForecastResponse>
    {
        public override void Configure()
        {
            Get(EndpointRoutes.WeatherForecast+ "/{id:guid}");
        }

        public override async Task HandleAsync(GetByIdWeatherForecastRequest req, CancellationToken ct)
        {
            var weatherForecast = await hybridCache.GetOrSetAsync(CacheKeys.WeatherForecastById(req.Id), async cancellationToken => { 
                return await context.WeatherForecasts.FirstOrDefaultAsync(w => w.Id == req.Id, cancellationToken: cancellationToken);
            }, token:ct); 

            if (weatherForecast == null)
            {
                await Send.NotFoundAsync(cancellation: ct);
            }
            var response = mapper.Map<WeatherForecastResponse>(weatherForecast);
            await Send.OkAsync(response, ct);
        }
    }
}
