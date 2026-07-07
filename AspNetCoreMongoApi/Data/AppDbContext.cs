using AspNetCoreMongoApi.Entities;
using AspNetCoreMongoApi.Outbox;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreMongoApi.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<WeatherForecast> WeatherForecasts { get; init; }
        public DbSet<Todo> Todos { get; init; }
        public DbSet<OutboxMessage> OutboxMessages { get; init; }
        public AppDbContext(DbContextOptions options): base(options){}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<OutboxMessage>().HasIndex(x => new { x.OccuredOnUtc, x.ProcessedOnUtc }).HasFilter($"\"{nameof(OutboxMessage.ProcessedOnUtc)}\" IS NULL").IncludeProperties(nameof(OutboxMessage.Id), nameof(OutboxMessage.Content), nameof(OutboxMessage.Type));
        }
    }
}
