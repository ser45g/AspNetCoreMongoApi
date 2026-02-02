using AspNetCoreMongoApi.Data;
using AspNetCoreMongoApi.Modules;
using AspNetCoreMongoApi.Profiles;
using Carter;
using Carter.ResponseNegotiators.Newtonsoft;
using Elastic.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Scalar.AspNetCore;
using System.Net.Sockets;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var mongoDbConnectionString = builder.Configuration.GetConnectionString("MongoDb");

if(mongoDbConnectionString == null)
    throw new ArgumentNullException(nameof(mongoDbConnectionString));

builder.Services.AddDbContext<MongoDbContext>(options =>
{
    options.UseMongoDB(mongoDbConnectionString);
});
builder.Services.AddAutoMapper(typeof(AppMappingProfile));



builder.Logging.AddElasticsearch();

builder.Services
    .AddOpenTelemetry()
    .WithTracing(opt =>
        opt.AddOtlpExporter().WithElasticDefaults().AddAspNetCoreInstrumentation().AddHttpClientInstrumentation()
    ).WithLogging(opt =>
        opt.AddOtlpExporter().WithElasticDefaults()).WithMetrics(opt => opt.AddOtlpExporter().WithElasticDefaults());


builder.Services.AddCarter(configurator: (config) =>
{
    config.WithResponseNegotiator<NewtonsoftJsonResponseNegotiator>();
    config.WithModule<WeatherForecastModule>();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapCarter();

using var scope = app.Services.CreateScope();

using (var context = scope.ServiceProvider.GetRequiredService<MongoDbContext>())
{
    context.Database.EnsureCreated();
}

app.Run();


