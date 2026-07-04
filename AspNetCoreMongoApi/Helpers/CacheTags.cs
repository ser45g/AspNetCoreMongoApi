namespace AspNetCoreMongoApi.Helpers
{
    public static class CacheTags
    {
        public static string TodoAuthorTag(string authorId) => $"todo-author-{authorId}";
    }
}
