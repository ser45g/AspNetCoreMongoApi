using AspNetCoreMongoApi.Data;
using AspNetCoreMongoApi.ErrorHandlers;
using AspNetCoreMongoApi.Options;
using AspNetCoreMongoApi.Profiles;
using AspNetCoreMongoApi.Validators;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Scalar.AspNetCore;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var mongoDbOptions = builder.Configuration.GetSection(MongoDbOptions.ConfigurationSection).Get<MongoDbOptions>();
var seqOptions = builder.Configuration.GetSection(SeqOptions.ConfigurationSection).Get<SeqOptions>();

if (mongoDbOptions == null || seqOptions == null)
{
    Console.Write(JsonSerializer.Serialize<MongoDbOptions>(mongoDbOptions));
    Console.WriteLine(JsonSerializer.Serialize<SeqOptions>(seqOptions));
    throw new ArgumentException("Configuration is incorrect");
}

var mongoDbOptionsValidator = new MongoDbOptionsValidator();
var mongoDbOptionsValidationResult = mongoDbOptionsValidator.Validate(mongoDbOptions);

if (!mongoDbOptionsValidationResult.IsValid)
{
    var errors = string.Join(", ", mongoDbOptionsValidationResult.Errors.Select(e => e.ErrorMessage));
    throw new InvalidOperationException($"Configuration validation failed: {errors}");
}

var seqOptionsValidator = new SeqOptionsValidator();
var seqOptionsValidationResult = seqOptionsValidator.Validate(seqOptions);

if (!seqOptionsValidationResult.IsValid)
{
    var errors = string.Join(", ", seqOptionsValidationResult.Errors.Select(e => e.ErrorMessage));
    throw new InvalidOperationException($"Configuration validation failed: {errors}");
}

builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"] = seqOptions.OtlpEndpoint;

builder.Services.AddDbContext<MongoDbContext>((options) =>
{
    options.UseMongoDB(mongoDbOptions.ConnectionString);
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

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
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


