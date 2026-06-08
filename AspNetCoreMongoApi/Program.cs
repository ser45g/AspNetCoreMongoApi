using AspNetCoreMongoApi.Data;
using AspNetCoreMongoApi.Extensions;
using AspNetCoreMongoApi.Options;
using AspNetCoreMongoApi.Profiles;
using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var mongoDbOptions = builder.Configuration.GetRequiredSection(MongoDbOptions.ConfigurationSection).Get<MongoDbOptions>();
ArgumentNullException.ThrowIfNull(mongoDbOptions);

var seqOptions = builder.Configuration.GetRequiredSection(SeqOptions.ConfigurationSection).Get<SeqOptions>();
ArgumentNullException.ThrowIfNull(seqOptions);

var authenticationOptions = builder.Configuration.GetRequiredSection(AuthenticationOptions.ConfigurationSection).Get<AuthenticationOptions>();
ArgumentNullException.ThrowIfNull(authenticationOptions);

builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"] = seqOptions.OtlpEndpoint;

builder.Services.AddOpenApiWithOIDC(authenticationOptions);

builder.Services.AddDbContext<MongoDbContext>((options) =>
{
    options.UseMongoDB(mongoDbOptions.ConnectionString);
});
builder.Services.AddAutoMapper(typeof(AppMappingProfile));

builder.Services.AddOptions<CorsOptions>().Bind(builder.Configuration.GetSection(CorsOptions.ConfigurationSection));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        string[] allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

        policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
    });
});

builder.Services.AddValidatedApplicationOptions();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.RequireHttpsMetadata = authenticationOptions.RequireHttps;
    options.Audience = authenticationOptions.Audience;
    options.MetadataAddress = authenticationOptions.MetadataAddress;
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidIssuer = authenticationOptions.ValidIssuer
    };
});

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


