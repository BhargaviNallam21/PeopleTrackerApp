

using AuthService.Application.Interfaces;
using AuthService.Application.Services;
using AuthService.Infrastructure.Data;
using AuthService.Infrastructure.Interfaces;
using AuthService.Infrastructure.Repositories;
using Azure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/authservice-log.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddDbContext<AuthDbContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("AuthDb")));


//builder.Services.AddDbContext<AuthDbContext>(opts =>
//opts.UseSqlServer(builder.Configuration.GetConnectionString("PeopleTrackerDBAuthConnectionString")));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthServicecls>();
builder.Services.AddControllers();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opts =>
    {
        opts.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"])),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
//builder.Configuration.AddAzureKeyVault(
//    new Uri($"https://<YourKeyVaultName>.vault.azure.net/"),
//    new DefaultAzureCredential());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddAzureKeyVault(
    new Uri($"https://{builder.Configuration["PeopleTrackerKeys"]}.vault.azure.net/"),
    new DefaultAzureCredential());

var key = builder.Configuration["JwtSettings:Key"];


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

// Enable routing
app.UseRouting();

// Enable authentication middleware (JWT tokens)
app.UseAuthentication();

// Enable authorization middleware
app.UseAuthorization();

// Map controllers (Web API endpoints)
app.MapControllers();



app.Run();


