using AspNetCoreMongoApi.Contracts.Todos.Request;
using AspNetCoreMongoApi.Contracts.Todos.Response;
using AspNetCoreMongoApi.Data;
using AspNetCoreMongoApi.Helpers;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using ZiggyCreatures.Caching.Fusion;

namespace AspNetCoreMongoApi.Endpoints.Todos
{
    public class UpdateTodoEndpoint(MongoDbContext context, IFusionCache hybridCache) : Endpoint<UpdateTodoRequest, TodoResponse>
    {
        public override void Configure()
        {
            Put(EndpointRoutes.Todos);
        }

        public override async Task HandleAsync(UpdateTodoRequest req, CancellationToken ct)
        {
            await hybridCache.RemoveAsync(CacheKeys.TodoById(req.Id), token: ct);

            var todo = await context.Todos.FirstOrDefaultAsync(w => w.Id == req.Id, cancellationToken: ct);

            if (todo == null)
            {
                await Send.NotFoundAsync(cancellation: ct);
                return;
            }

            todo.Title = req.Title;
            todo.IsComplete = req.IsComplete;
            todo.From = req.From;
            todo.To = req.To;
            todo.Description = req.Description;
            todo.UpdatedAt= DateTime.UtcNow;
           
            await context.SaveChangesAsync(ct);

            await Send.NoContentAsync(ct);
        }
    }
}
