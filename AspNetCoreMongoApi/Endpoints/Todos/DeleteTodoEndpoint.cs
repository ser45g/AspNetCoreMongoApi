using AspNetCoreMongoApi.Contracts.Todos.Request;
using AspNetCoreMongoApi.Data;
using AspNetCoreMongoApi.Entities;
using AspNetCoreMongoApi.Helpers;
using Elastic.Clients.Elasticsearch;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using ZiggyCreatures.Caching.Fusion;

namespace AspNetCoreMongoApi.Endpoints.Todos
{
   
    public class DeleteTodoEndpoint(AppDbContext context, ElasticsearchClient elasticsearchClient, IFusionCache hybridCache) : Endpoint<DeleteTodoRequest>
    {
        public override void Configure()
        {
            Delete(EndpointRoutes.Todos + "/{id:guid}");
            Description(x => x.ClearDefaultAccepts());
        }

        public override async Task HandleAsync(DeleteTodoRequest req, CancellationToken ct)
        {
            var todo = await context.Todos.FirstOrDefaultAsync(t=>t.Id==req.Id);

            if (todo == null) {
                await Send.NotFoundAsync(ct);
                return;
            }

            context.Todos.Remove(todo);

            await using var trans = await context.Database.BeginTransactionAsync(ct);

            try
            {
                await context.SaveChangesAsync(ct);

                var elasticSearchResponse = await elasticsearchClient.DeleteAsync<Todo>(req.Id, x => x.Index("todos"), cancellationToken: ct);

                if (elasticSearchResponse.IsValidResponse)
                {
                    await trans.CommitAsync(ct);

                    await hybridCache.RemoveAsync(CacheKeys.TodoById(req.Id), token: ct);

                    await Send.NoContentAsync(ct);
                    return;
                }
                await trans.RollbackAsync(ct);
            }
            catch(Exception ex) 
            {
                await trans.RollbackAsync(ct);
            }

            await Send.ResultAsync(Results.InternalServerError());
        }
    }
}
