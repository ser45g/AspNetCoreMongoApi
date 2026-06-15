namespace AspNetCoreMongoApi.Contracts.Todos.Request
{
    public record class UpdateTodoRequest(Guid Id, string Title, DateTime? From, DateTime? To, string? Description, bool IsComplete=false);

}
