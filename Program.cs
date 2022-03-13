using System.Net.Mime;
using System.Text;
using System.Text.Json;
using CalenTaskApi.Respositories;
using CalenTaskApi.Settings;
using CalenTaskApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Swashbuckle.AspNetCore.Filters;
using CalenTaskApi.Service;
using Microsoft.AspNetCore.Authentication.Cookies;

var AllowOriginsPolicy = "_AllowOriginsPolicy";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

var mongoDbSettings = builder.Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();

builder.Services.AddSingleton<IMongoClient>(serviceProvider => 
{    
    return new MongoClient(mongoDbSettings.ConnectionString);
});

builder.Services.AddSingleton<IUsersRepository, MongoDbUsersRepository>();
builder.Services.AddSingleton<ITodoRepository, MongoDbTodoRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IReadJwtTokenService, ReadJwtTokenService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddHealthChecks()
    .AddMongoDb(mongoDbSettings.ConnectionString,
     name: "calentask", 
     timeout: TimeSpan.FromSeconds(3),
     tags: new[] { "ready" });

builder.Services.AddControllers(options => {
    options.SuppressAsyncSuffixInActionNames = false;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JwtSettings:TokenKey").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddAuthentication(options => {
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
        .AddCookie(options =>
        {
            options.LoginPath = "/users/google/login"; 
        })
        .AddGoogle(options =>
        {
            options.ClientId = builder.Configuration.GetSection("GoogleOauthSettings:ClientId").Value;
            options.ClientSecret = builder.Configuration.GetSection("GoogleOauthSettings:ClientSecret").Value;
        });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme(\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowOriginsPolicy,
    builder =>
    {
        builder.WithOrigins("https://localhost:7147", "http://localhost:3000")
        .AllowAnyHeader();
    });

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if(app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors(AllowOriginsPolicy);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health/ready", new HealthCheckOptions{
    Predicate = (check) => check.Tags.Contains("ready"),
    ResponseWriter = async(context, report) =>
    {
        var result = JsonSerializer.Serialize(
            new{
                status = report.Status.ToString(),
                checks = report.Entries.Select(entry => new {
                    name = entry.Key,
                    status = entry.Value.Status.ToString(),
                    exception = entry.Value.Exception != null ? entry.Value.Exception.Message : "none",
                    duration = entry.Value.Duration.ToString()
                })
            }
        );
        context.Response.ContentType = MediaTypeNames.Application.Json;
        await context.Response.WriteAsync(result);
    }
});

app.MapHealthChecks("/health/live", new HealthCheckOptions{
    Predicate = (__) => false
});

app.Run();
