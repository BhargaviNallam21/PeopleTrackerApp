using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TrackerService.Application;
using TrackerService.Application.Services;
using TrackerService.Domain.Interface;
using TrackerService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// JWT Authentication Setup
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "http://localhost:31034"; // AuthService URL
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };
    });

// DI Setup
builder.Services.AddScoped<ITrackerService, TrackerServicecls>();
builder.Services.AddScoped<IPeopleServiceClient, PeopleServiceClient>();

builder.Services.AddHttpClient<IPeopleServiceClient, PeopleServiceClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:41412"); // PeopleService URL
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();