namespace AspNetCoreMongoApi.Contracts.Todos.Request
{

    public record class CreateTodoRequest(string Title, DateTime? From, DateTime? To, string? Description=null, bool IsComplete=true);
}
