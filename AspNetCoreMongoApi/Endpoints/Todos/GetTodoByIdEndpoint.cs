using AspNetCoreMongoApi.Contracts.Todos.Request;
using AspNetCoreMongoApi.Contracts.Todos.Response;
using AspNetCoreMongoApi.Contracts.WeatherForecasts.Request;
using AspNetCoreMongoApi.Contracts.WeatherForecasts.Response;
using AspNetCoreMongoApi.Data;
using AspNetCoreMongoApi.Helpers;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using ZiggyCreatures.Caching.Fusion;

namespace AspNetCoreMongoApi.Endpoints.Todos
{
   
    public class GetTodoByIdEndpoint(AutoMapper.IMapper mapper, MongoDbContext context, IFusionCache hybridCache) : Endpoint<GetByIdTodoRequest, TodoResponse>
    {
        public override void Configure()
        {
            Get(EndpointRoutes.Todos + "/{id:guid}");
        }

        public override async Task HandleAsync(GetByIdTodoRequest req, CancellationToken ct)
        {
            var todo = await hybridCache.GetOrSetAsync(CacheKeys.TodoById(req.Id), async cancellationToken => {
                return await context.Todos.FirstOrDefaultAsync(w => w.Id == req.Id, cancellationToken: cancellationToken);
            }, token: ct);

            if (todo == null)
            {
                await Send.NotFoundAsync(cancellation: ct);
            }
            var response = mapper.Map<TodoResponse>(todo);
            await Send.OkAsync(response, ct);
        }
    }

}
