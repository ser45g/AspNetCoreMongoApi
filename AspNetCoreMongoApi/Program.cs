using AspNetCoreMongoApi.Data;
using AspNetCoreMongoApi.Modules;
using AspNetCoreMongoApi.Profiles;
using Carter;
using Carter.ResponseNegotiators.Newtonsoft;
using Microsoft.EntityFrameworkCore;
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

builder.Services.AddLogging();

builder.Services.AddOpenTelemetry().ConfigureResource(resource =>
{
    resource.AddService("WeatherForecast");
}).WithMetrics(metrics =>
{
    metrics.AddAspNetCoreInstrumentation().AddHttpClientInstrumentation();

    metrics.AddPrometheusExporter();
}).WithTracing(tracing =>
{
    tracing.AddAspNetCoreInstrumentation().AddHttpClientInstrumentation().AddEntityFrameworkCoreInstrumentation();

    tracing.AddOtlpExporter();
});

builder.Logging.AddOpenTelemetry(logging => logging.AddOtlpExporter());


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

app.UseOpenTelemetryPrometheusScrapingEndpoint();
app.MapCarter();

using var scope = app.Services.CreateScope();

using (var context = scope.ServiceProvider.GetRequiredService<MongoDbContext>())
{
    context.Database.EnsureCreated();
}

app.Run();


