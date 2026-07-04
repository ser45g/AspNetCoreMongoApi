using AspNetCoreMongoApi.Consumers.Keycloak.Events;
using AspNetCoreMongoApi.Data;
using AspNetCoreMongoApi.Entities;
using AspNetCoreMongoApi.Helpers;
using Elastic.Clients.Elasticsearch;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using ZiggyCreatures.Caching.Fusion;

namespace AspNetCoreMongoApi.Consumers.Keycloak
{
    public class UserDeletedConsumer(AppDbContext dbContext, IFusionCache hybridCache, ElasticsearchClient elasticsearchClient) : IConsumer<KeycloakAdminEventMessage>
    {       
        public async Task Consume(ConsumeContext<KeycloakAdminEventMessage> context)
        {
            var message = context.Message;

            var userId = message.ResourceId;

            var ct = context.CancellationToken;

            var todos = await dbContext.Todos.Where(t => t.AuthorId == userId).ToListAsync(ct);

            if (todos==null || todos.Count==0)
            {
                return;
            }

            var todosIds = todos.Select(t => FieldValue.String(t.Id.ToString())).ToList();

            dbContext.Todos.RemoveRange(todos);

            using var trans = await dbContext.Database.BeginTransactionAsync(ct); //can't use <await using>, basically, that early return in the successful scenario will not work correctly, the last throw still be executed. Which is weird. I tried other code in that line (maybe there's something special about throwing in this case I thought) but no difference, it still gets executed. So, idk, it's weird

            try
            {
                await dbContext.SaveChangesAsync();

                var elasticSearchResponse = await elasticsearchClient.DeleteByQueryAsync<Todo>(ElasticSearchIndecies.TodosIndex, d => d
                    .Query(q => q
                        .Terms(t => t
                            .Field(f => f.Id)
                            .Terms(f=>f.Value(todosIds))
                    )
                )
                .Refresh(true), ct);

                if (elasticSearchResponse.IsValidResponse)
                {
                    await trans.CommitAsync(ct);

                    await hybridCache.RemoveByTagAsync(CacheTags.TodoAuthorTag(userId), token:ct);

                    return;
                }
                await trans.RollbackAsync(ct);
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync(ct);
            }

            throw new Exception($"Failed to delete todos for user {userId}.");
        }
    }
}
