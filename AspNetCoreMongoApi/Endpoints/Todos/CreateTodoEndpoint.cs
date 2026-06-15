using AspNetCoreMongoApi.Contracts.Todos.Request;
using AspNetCoreMongoApi.Contracts.Todos.Response;
using AspNetCoreMongoApi.Data;
using AspNetCoreMongoApi.Entities;
using AspNetCoreMongoApi.Helpers;
using FastEndpoints;


namespace AspNetCoreMongoApi.Endpoints.Todos
{
    public class CreateTodoEndpoint(AutoMapper.IMapper mapper, MongoDbContext context) : Endpoint<CreateTodoRequest, TodoResponse>
    {
        public override void Configure()
        {
            Post(EndpointRoutes.Todos);
        }

        public override async Task HandleAsync(CreateTodoRequest req, CancellationToken ct)
        {
            var todo = mapper.Map<Todo>(req);

            context.Todos.Add(todo);

            await context.SaveChangesAsync();

            TodoResponse response = mapper.Map<TodoResponse>(req);

            await Send.CreatedAtAsync<GetTodoByIdEndpoint>(new { id = response.Id }, response, cancellation: ct);
        }
    }
}
