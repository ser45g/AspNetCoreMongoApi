using AspNetCoreMongoApi.Data;
using AspNetCoreMongoApi.ErrorHandlers;
using AspNetCoreMongoApi.Profiles;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Scalar.AspNetCore;

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

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyMethod().AllowAnyHeader().WithOrigins();
    });
});
builder.Services.AddLogging();

builder.Services.AddOpenTelemetry().ConfigureResource(resource =>
{
    resource.AddService("WeatherForecast");
}).WithMetrics(metrics =>
{
    metrics.AddAspNetCoreInstrumentation().AddHttpClientInstrumentation();

    metrics.AddOtlpExporter();
}).WithTracing(tracing =>
{
    tracing.AddAspNetCoreInstrumentation().AddHttpClientInstrumentation().AddEntityFrameworkCoreInstrumentation();

    tracing.AddOtlpExporter();
});

builder.Logging.AddOpenTelemetry(logging => logging.AddOtlpExporter());

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddFastEndpoints();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseCors();
app.MapFastEndpoints();

app.UseExceptionHandler();
app.UseStatusCodePages();

using var scope = app.Services.CreateScope();

using (var context = scope.ServiceProvider.GetRequiredService<MongoDbContext>())
{
    context.Database.EnsureCreated();
}

app.Run();


