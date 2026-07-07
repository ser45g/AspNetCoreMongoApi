namespace AspNetCoreMongoApi.Consumers.Cleanup
{
    public record class CleanupByUserIdEvent(string UserId);
}
