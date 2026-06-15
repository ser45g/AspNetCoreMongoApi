using AspNetCoreMongoApi.Data;
using AspNetCoreMongoApi.Extensions;
using AspNetCoreMongoApi.Options;
using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.IdentityModel.Tokens;
using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Backplane.StackExchangeRedis;
using ZiggyCreatures.Caching.Fusion.Serialization.SystemTextJson;

var builder = WebApplication.CreateBuilder(args);

var mongoDbOptions = builder.Configuration.GetRequiredSection(MongoDbOptions.ConfigurationSection).Get<MongoDbOptions>();
ArgumentNullException.ThrowIfNull(mongoDbOptions);

var seqOptions = builder.Configuration.GetRequiredSection(SeqOptions.ConfigurationSection).Get<SeqOptions>();
ArgumentNullException.ThrowIfNull(seqOptions);

var redisCacheOptions = builder.Configuration.GetRequiredSection(RedisOptions.ConfigurationSection).Get<RedisOptions>();
ArgumentNullException.ThrowIfNull(redisCacheOptions);

var authenticationOptions = builder.Configuration.GetRequiredSection(AuthenticationOptions.ConfigurationSection).Get<AuthenticationOptions>();
ArgumentNullException.ThrowIfNull(authenticationOptions);

builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"] = seqOptions.OtlpEndpoint;

builder.Services.AddOpenApiWithOIDC(authenticationOptions);

builder.Services.AddDbContext<MongoDbContext>((options) =>
{
    options.UseMongoDB(mongoDbOptions.ConnectionString);
});

builder.Services.AddFusionCache().WithDefaultEntryOptions(o =>
{
    o.Duration = TimeSpan.FromMinutes(5);
    o.DistributedCacheDuration = TimeSpan.FromMinutes(8);
}).WithSerializer(new FusionCacheSystemTextJsonSerializer())
.WithDistributedCache(new RedisCache(new RedisCacheOptions() { Configuration = redisCacheOptions.Configuration }))
.WithBackplane(new RedisBackplane(new RedisBackplaneOptions() {
    Configuration = redisCacheOptions.Configuration,
}));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        string[] allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

        policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
    });
});

builder.Services.AddValidatedApplicationOptions();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.RequireHttpsMetadata = authenticationOptions.RequireHttps;
    options.Audience = authenticationOptions.Audience;
    options.MetadataAddress = authenticationOptions.MetadataAddress;
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidIssuer = authenticationOptions.ValidIssuer
    };
});
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Admin", policy => policy.RequireRole("admin"));

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

app.MapFastEndpoints(c =>
{
    c.Errors.UseProblemDetails();
});

using var scope = app.Services.CreateScope();

using (var context = scope.ServiceProvider.GetRequiredService<MongoDbContext>())
{
    context.Database.EnsureCreated();
}

app.Run();


