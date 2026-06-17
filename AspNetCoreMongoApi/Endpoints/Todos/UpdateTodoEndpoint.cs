using AspNetCoreMongoApi.Contracts.Todos.Request;
using AspNetCoreMongoApi.Contracts.Todos.Response;
using AspNetCoreMongoApi.Data;
using AspNetCoreMongoApi.Extensions.Mappers;
using AspNetCoreMongoApi.Helpers;
using Elastic.Clients.Elasticsearch;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using ZiggyCreatures.Caching.Fusion;

namespace AspNetCoreMongoApi.Endpoints.Todos
{
    public class UpdateTodoEndpoint(AppDbContext context,ElasticsearchClient elasticsearchClient,  IFusionCache hybridCache) : Endpoint<UpdateTodoRequest, TodoResponse>
    {
        public override void Configure()
        {
            Put(EndpointRoutes.Todos);
        }

        public override async Task HandleAsync(UpdateTodoRequest req, CancellationToken ct)
        {
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

            await using var trans = await context.Database.BeginTransactionAsync(ct);

            try
            {
                await context.SaveChangesAsync(ct);

                var elasticSearchResponse = await elasticsearchClient.IndexAsync(todo, x => x.Index("todos"), cancellationToken: ct);

                if (elasticSearchResponse.IsValidResponse)
                {
                    await trans.CommitAsync(ct);

                    await hybridCache.RemoveAsync(CacheKeys.TodoById(req.Id), token: ct);

                    await Send.OkAsync(todo.ToTodoResponse(), cancellation: ct);
                    return;
                }
                await trans.RollbackAsync(ct);
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync(ct);
            }
            await Send.ResultAsync(Results.InternalServerError());
        }
    }
}
