using AspNetCoreMongoApi.Contracts.Todos.Request;
using AspNetCoreMongoApi.Contracts.Todos.Response;
using AspNetCoreMongoApi.Data;
using AspNetCoreMongoApi.Extensions.Mappers;
using AspNetCoreMongoApi.Helpers;
using FastEndpoints;


namespace AspNetCoreMongoApi.Endpoints.Todos
{
    public class CreateTodoEndpoint(MongoDbContext context) : Endpoint<CreateTodoRequest, TodoResponse>
    {
        public override void Configure()
        {
            Post(EndpointRoutes.Todos);
        }

        public override async Task HandleAsync(CreateTodoRequest req, CancellationToken ct)
        {
            var todo = req.ToTodo();

            context.Todos.Add(todo);

            await context.SaveChangesAsync();

            TodoResponse response = todo.ToTodoResponse();

            await Send.CreatedAtAsync<GetTodoByIdEndpoint>(new { id = response.Id }, response, cancellation: ct);
        }
    }
}
