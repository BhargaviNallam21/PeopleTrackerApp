using Azure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using PeopleService.Application.Services;
using PeopleService.Infrastructure.Data;
using PeopleService.Infrastructure.Repositories;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ---------- Serilog Logging ---------- //
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/peopleservice-log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// ---------- EF Core ---------- //
//builder.Services.AddDbContext<PeopleDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("PeopleDb")));
builder.Configuration.AddUserSecrets<Program>();
builder.Services.AddDbContext<PeopleDbContext>(opts =>
opts.UseSqlServer(builder.Configuration.GetConnectionString("PeopleTrackerProdDbconnectionstring")));

// ---------- Azure Key Vault ---------- //
builder.Configuration.AddAzureKeyVault(
    new Uri($"https://{builder.Configuration["PeopleTrackerKeys"]}.vault.azure.net/"),
    new DefaultAzureCredential());

// Get JWT Key from Key Vault



var key = builder.Configuration["JwtSettings:Key"];
if (string.IsNullOrEmpty(key))
    throw new Exception("JWT Key not loaded from Azure Key Vault");

// ---------- JWT Authentication ---------- //
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// ---------- Dependency Injection ---------- //
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IPersonService, PersonService>();

builder.Services.AddControllers();

// ---------- Swagger + JWT Auth ---------- //
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PeopleService API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Enter 'Bearer {token}'"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] { }
        }
    });
});

var app = builder.Build();

// ---------- Middleware ---------- //
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // ✅ Shows full error details in dev
}
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();