namespace AspNetCoreMongoApi.Contracts.Todos.Request
{

    public record class CreateTodoRequest(string Title, bool IsComplete, DateTime? From, DateTime? To, string? Description);
}
