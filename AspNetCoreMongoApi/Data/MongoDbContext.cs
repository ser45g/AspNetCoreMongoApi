using AspNetCoreMongoApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;
using System.Threading.Tasks;

namespace AspNetCoreMongoApi.Data
{
  

    public class MongoDbContext : DbContext
    {
        public DbSet<WeatherForecast> WeatherForecasts { get; init; }

        public MongoDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<WeatherForecast>().ToCollection("weatherForecasts");
        }
    }
}
