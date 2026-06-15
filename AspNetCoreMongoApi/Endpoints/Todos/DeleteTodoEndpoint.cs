using AspNetCoreMongoApi.Contracts.Todos.Request;
using AspNetCoreMongoApi.Data;
using AspNetCoreMongoApi.Entities;
using AspNetCoreMongoApi.Helpers;
using FastEndpoints;
using ZiggyCreatures.Caching.Fusion;

namespace AspNetCoreMongoApi.Endpoints.Todos
{
   
    public class DeleteTodoEndpoint(MongoDbContext context, IFusionCache hybridCache) : Endpoint<DeleteTodoRequest>
    {
        public override void Configure()
        {
            Delete(EndpointRoutes.Todos + "/{id:guid}");
            Description(x => x.ClearDefaultAccepts());
        }

        public override async Task HandleAsync(DeleteTodoRequest req, CancellationToken ct)
        {
            await hybridCache.RemoveAsync(CacheKeys.TodoById(req.Id), token: ct);

            context.Todos.Remove(new Todo() { Id = req.Id,Title="",CreatedAt=DateTime.MinValue });

            await context.SaveChangesAsync();

            await Send.NoContentAsync(ct);
        }
    }
}
