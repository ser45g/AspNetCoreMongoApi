namespace AspNetCoreMongoApi.Entities
{
    public class Todo
    {
        public Guid Id { get; set; }

        public required string Title { get; set; }

        public bool IsComplete { get; set; }

        public required DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        //public required string AuthorId { get; set; }

        public string? Description { get; set; }
    }
}
