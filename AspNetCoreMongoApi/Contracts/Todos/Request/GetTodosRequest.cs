namespace AspNetCoreMongoApi.Contracts.Todos.Request
{
    public record class GetTodosRequest(
        DateTime? MinTo = null,
        DateTime? MaxTo = null,

        DateTime? MinFrom = null,
        DateTime? MaxFrom = null,

        DateTime? MinCreatedAt = null,
        DateTime? MaxCreatedAt = null,

        DateTime? MinUpdatedAt = null,
        DateTime? MaxUpdatedAt = null,

        bool? IsComplete=null,

        string? SearchTerm = null,

        string? SortColumn = null,
        bool? SortAsc = true,

        int? PageSize = null,
        Guid? Cursor = null);
}
