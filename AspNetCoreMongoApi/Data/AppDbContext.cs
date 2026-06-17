using AspNetCoreMongoApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreMongoApi.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<WeatherForecast> WeatherForecasts { get; init; }
        public DbSet<Todo> Todos { get; init; }

        public AppDbContext(DbContextOptions options): base(options){}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
