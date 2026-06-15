namespace AspNetCoreMongoApi.Contracts.Common.Response
{
    public record class CursorPaginationResponse<T>(Guid? NextCursor, T Data, int Count, int TotalCount);

}
