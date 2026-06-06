using AspNetCoreMongoApi.Data;
using AspNetCoreMongoApi.ErrorHandlers;
using AspNetCoreMongoApi.Extensions;
using AspNetCoreMongoApi.Options;
using AspNetCoreMongoApi.Profiles;
using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var mongoDbOptions = builder.Configuration.GetRequiredSection(MongoDbOptions.ConfigurationSection).Get<MongoDbOptions>();
ArgumentNullException.ThrowIfNull(mongoDbOptions);

var seqOptions = builder.Configuration.GetRequiredSection(SeqOptions.ConfigurationSection).Get<SeqOptions>();
ArgumentNullException.ThrowIfNull(seqOptions);

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

builder.Services.AddValidatedApplicationOptions();

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

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

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


