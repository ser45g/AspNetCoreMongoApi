using AspNetCoreMongoApi.Entities;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace AspNetCoreMongoApi.Data
{
  

    public class MongoDbContext : DbContext
    {
        public DbSet<WeatherForecast> WeatherForecasts { get; init; }

        public MongoDbContext(DbContextOptions options): base(options){}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<WeatherForecast>().ToCollection("weatherForecasts");
        }
    }
}
