using AspNetCoreMongoApi.Contracts.Todos.Request;
using AspNetCoreMongoApi.Contracts.Todos.Response;
using AspNetCoreMongoApi.Data;
using AspNetCoreMongoApi.Extensions.Mappers;
using AspNetCoreMongoApi.Helpers;
using Elastic.Clients.Elasticsearch;
using FastEndpoints;
using System.Security.Claims;

namespace AspNetCoreMongoApi.Endpoints.Todos
{
    public class CreateTodoEndpoint(AppDbContext context, ElasticsearchClient elasticsearchClient) : Endpoint<CreateTodoRequest, TodoResponse>
    {
        public override void Configure()
        {
            Post(EndpointRoutes.Todos);
        }

        public override async Task HandleAsync(CreateTodoRequest req, CancellationToken ct)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string? email = User.FindFirstValue(ClaimTypes.Email);
            string? name = User.FindFirstValue("name");

            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(name))
            {
                await Send.UnauthorizedAsync(ct);
                return;
            }

            var todo = req.ToTodo(userId);
            context.Todos.Add(todo);

            await using var trans = await context.Database.BeginTransactionAsync(ct);

            try
            {
                await context.SaveChangesAsync(ct);

                var elasticSearchResponse = await elasticsearchClient.IndexAsync(todo, x => x.Index(ElasticSearchIndecies.TodosIndex), cancellationToken:ct);

                if (elasticSearchResponse.IsValidResponse)
                {
                    TodoResponse response = todo.ToTodoResponse(new Entities.User(userId, name, email));

                    await trans.CommitAsync(ct);

                    await Send.CreatedAtAsync<GetTodoByIdEndpoint>(new { id = response.Id }, response, cancellation: ct);
                    return;
                }
                await trans.RollbackAsync(ct);
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                await trans.RollbackAsync(ct);
            }
            await Send.ResultAsync(Results.InternalServerError());
        }
    }
}
