

//using AuthService.Application.Interfaces;
//using AuthService.Application.Services;
//using AuthService.Infrastructure.Data;
//using AuthService.Infrastructure.Interfaces;
//using AuthService.Infrastructure.Repositories;
//using Azure.Identity;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using Serilog;
//using System.Text;


//var builder = WebApplication.CreateBuilder(args);
//Log.Logger = new LoggerConfiguration()
//    .ReadFrom.Configuration(builder.Configuration)
//    .Enrich.FromLogContext()
//    .WriteTo.Console()
//    .WriteTo.File("Logs/authservice-log.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7)
//    .CreateLogger();

//builder.Host.UseSerilog();

////builder.Services.AddDbContext<AuthDbContext>(opts =>
////    opts.UseSqlServer(builder.Configuration.GetConnectionString("AuthDb")));

////builder.Configuration.AddUserSecrets<Program>();
////builder.Services.AddDbContext<AuthDbContext>(opts =>
////opts.UseSqlServer(builder.Configuration.GetConnectionString("PeopleTrackerProdDbconnectionstring")));
//builder.Services.AddScoped<IUserRepository, UserRepository>();
//builder.Services.AddScoped<IAuthService, AuthServicecls>();
//builder.Services.AddControllers();
//builder.Configuration.AddUserSecrets<Program>();
//var keyVaultName = builder.Configuration["PeopleTrackerKeys"];

//if (!string.IsNullOrEmpty(keyVaultName))
//{
//    builder.Configuration.AddAzureKeyVault(
//        new Uri($"https://{keyVaultName}.vault.azure.net/"),
//        new DefaultAzureCredential());
//}

//// Now build the configuration AFTER adding Key Vault
////var configuration = builder.Configuration.Build();

//// Use the built configuration from here on
//builder.Services.AddDbContext<AuthDbContext>(opts =>
//    opts.UseSqlServer(builder.Configuration.GetConnectionString("PeopleTrackerProdDbconnectionstring")));
//var key = builder.Configuration["JwtSettings:Key"];

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(opts =>
//    {
//        opts.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuerSigningKey = true,
//            IssuerSigningKey = new SymmetricSecurityKey(
//                Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"])),
//            ValidateIssuer = false,
//            ValidateAudience = false
//        };
//    });
////builder.Configuration.AddAzureKeyVault(
////    new Uri($"https://<YourKeyVaultName>.vault.azure.net/"),
////    new DefaultAzureCredential());

//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

////builder.Configuration.AddAzureKeyVault(
////    new Uri($"https://{builder.Configuration["PeopleTrackerKeys"]}.vault.azure.net/"),
////    new DefaultAzureCredential());




//var app = builder.Build();

////if (app.Environment.IsDevelopment())
////{
////    app.UseSwagger();
////    app.UseSwaggerUI();
////}

//app.UseSwagger();
//app.UseSwaggerUI();
//// Configure the HTTP request pipeline.
//app.UseHttpsRedirection();

//// Enable routing
//app.UseRouting();

//// Enable authentication middleware (JWT tokens)
//app.UseAuthentication();

//// Enable authorization middleware
//app.UseAuthorization();

//// Map controllers (Web API endpoints)
//app.MapControllers();



//app.Run();

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

// Setup logging
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/authservice-log.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7)
    .CreateLogger();

builder.Host.UseSerilog();

// Load User Secrets
builder.Configuration.AddUserSecrets<Program>();

// Read Key Vault Name from user secrets/appsettings
var keyVaultName1 = builder.Configuration["KeyVaultName"];

// Add Azure Key Vault (if name exists)
if (!string.IsNullOrEmpty(keyVaultName1))
{
    builder.Configuration.AddAzureKeyVault(
        new Uri($"https://{keyVaultName1}.vault.azure.net/"),
        new DefaultAzureCredential());
}

// ✅ Rebuild the configuration to include Key Vault values
var config = builder.Configuration;

// ✅ Read values safely after config is built
//var jwtKey = config["JwtSettings:Key"];
//var connStr = config.GetConnectionString("PeopleTrackerProdDbconnectionstring");

//// 🧨 Add validation and throw if secrets are missing
//if (string.IsNullOrEmpty(jwtKey))
//    throw new Exception("JWT secret (JwtSettings:Key) not found in configuration.");

//if (string.IsNullOrEmpty(connStr))
//    throw new Exception("Connection string (PeopleTrackerProdDbconnectionstring) not found in configuration.");

//// DB Context
//builder.Services.AddDbContext<AuthDbContext>(opts =>
//    opts.UseSqlServer(connStr));

try
{
    var jwtKey = config["JwtSettings:Key"];
    if (string.IsNullOrEmpty(jwtKey))
        throw new Exception("❌ JWT secret not found");

    var connStr = config.GetConnectionString("PeopleTrackerProdDbconnectionstring");
    if (string.IsNullOrEmpty(connStr))
        throw new Exception("❌ Connection string not found");

    builder.Services.AddDbContext<AuthDbContext>(opts => opts.UseSqlServer(connStr));
}
catch (Exception ex)
{
    Log.Error(ex, "Startup failure");
    throw;
}


// Register services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthServicecls>();

// Add controllers
builder.Services.AddControllers();

// JWT Auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opts =>
    {
        var jwtKey = config["JwtSettings:Key"];
        opts.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

