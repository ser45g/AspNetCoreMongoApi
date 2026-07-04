using AspNetCoreMongoApi.Consumers;
using AspNetCoreMongoApi.Data;
using AspNetCoreMongoApi.Entities;
using AspNetCoreMongoApi.Extensions;
using AspNetCoreMongoApi.Helpers;
using AspNetCoreMongoApi.Options;
using AspNetCoreMongoApi.Services;
using Elastic.Clients.Elasticsearch;
using FastEndpoints;
using FluentValidation;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.IdentityModel.Tokens;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Backplane.StackExchangeRedis;
using ZiggyCreatures.Caching.Fusion.Serialization.SystemTextJson;

var builder = WebApplication.CreateBuilder(args);

var dbOptions = builder.Configuration.GetRequiredSection(DbOptions.ConfigurationSection).Get<DbOptions>();
ArgumentNullException.ThrowIfNull(dbOptions);

var redisCacheOptions = builder.Configuration.GetRequiredSection(RedisOptions.ConfigurationSection).Get<RedisOptions>();
ArgumentNullException.ThrowIfNull(redisCacheOptions);

var authenticationOptions = builder.Configuration.GetRequiredSection(AuthenticationOptions.ConfigurationSection).Get<AuthenticationOptions>();
ArgumentNullException.ThrowIfNull(authenticationOptions);

var keycloakClientOptions = builder.Configuration.GetRequiredSection(KeycloakClientOptions.ConfigurationSection).Get<KeycloakClientOptions>();
ArgumentNullException.ThrowIfNull(keycloakClientOptions);

var elasticSearchOptions= builder.Configuration.GetRequiredSection(ElasticSearchOptions.ConfigurationSection).Get<ElasticSearchOptions>();
ArgumentNullException.ThrowIfNull(elasticSearchOptions);

var rabbitMqOptions= builder.Configuration.GetRequiredSection(RabbitMqOptions.ConfigurationSection).Get<RabbitMqOptions>();
ArgumentNullException.ThrowIfNull(rabbitMqOptions);

builder.Services.AddOpenApiWithOIDC(authenticationOptions);

builder.Services.AddDbContext<AppDbContext>((options) =>
{
    options.UseNpgsql(dbOptions.ConnectionString);
});

builder.Services.AddKeycloakClientServices(keycloakClientOptions);

builder.Services.AddElasticSearchClient(elasticSearchOptions);

builder.Services.AddFusionCache().WithDefaultEntryOptions(o =>
{
    o.Duration = TimeSpan.FromMinutes(5);
    o.DistributedCacheDuration = TimeSpan.FromMinutes(8);
}).WithSerializer(new FusionCacheSystemTextJsonSerializer())
.WithDistributedCache(new RedisCache(new RedisCacheOptions() { Configuration = redisCacheOptions.Configuration }))
.WithBackplane(new RedisBackplane(new RedisBackplaneOptions() {
    Configuration = redisCacheOptions.Configuration,
}));
builder.Services.AddHealthChecks();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        string[] allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

        policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
    });
});

builder.Services.AddAppMessaging(rabbitMqOptions, keycloakClientOptions.Realm);

builder.Services.AddLogging();

builder.Services.AddOpenTelemetry().ConfigureResource(resource =>
{
    resource.AddService("AspNetCoreMongoApi");
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

builder.Services.AddValidatedApplicationOptions();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.RequireHttpsMetadata = authenticationOptions.RequireHttps;
    options.Audience = authenticationOptions.Audience;
    options.MetadataAddress = authenticationOptions.MetadataAddress;
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidIssuer = authenticationOptions.ValidIssuer
    };
});
builder.Services.AddAuthorization(o =>
{
    o.AddPolicy("Admin", policy => policy.RequireRole("admin"));
});

builder.Services.AddSingleton<IEmailService, EmailService>();

builder.Services.AddProblemDetails();

builder.Services.AddFastEndpoints();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapOpenScalarReferenceWithOIDC(authenticationOptions);
}

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseCors();

app.MapHealthChecks("/health");

app.MapFastEndpoints(c =>
{
    c.Errors.UseProblemDetails();
});

using var scope = app.Services.CreateScope();

using (var context = scope.ServiceProvider.GetRequiredService<AppDbContext>())
{
    context.Database.EnsureCreated();
}

var elasticsearchClient = scope.ServiceProvider.GetRequiredService<ElasticsearchClient>();

var elasticsearchExistsResponse = await elasticsearchClient.Indices.ExistsAsync(ElasticSearchIndecies.TodosIndex);

if (!elasticsearchExistsResponse.IsValidResponse)
    throw new Exception($"Elastic Search index \"{ElasticSearchIndecies.TodosIndex}\" request was unsuccessful.");

if (!elasticsearchExistsResponse.Exists)
{
    var createIndexResponse = await elasticsearchClient.Indices.CreateAsync<Todo>(ElasticSearchIndecies.TodosIndex, c => c
        .Mappings(m => m
            .Properties(p => p
                .Text(t=>t.Title)
                .Text(t=> t.Description)
                .Keyword(t=>t.Id)
                .Text(t=>t.AuthorId)
                .Boolean(t=>t.IsComplete)
                .Date(t=> t.CreatedAt)
                .Date(t=> t.UpdatedAt)
                .Date(t => t.From)
                .Date(t => t.To)
        )
    ));

    if(!createIndexResponse.IsValidResponse)
        throw new Exception("Failed to create Elasticsearch index for todos");
}

app.Run();


