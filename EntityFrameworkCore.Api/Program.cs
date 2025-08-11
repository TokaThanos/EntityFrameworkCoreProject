using EntityFrameworkCore.Application.Interfaces;
using EntityFrameworkCore.Application.Services;
using EntityFrameworkCore.Data;
using EntityFrameworkCore.Data.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region EnvironmentVariableConfiguration
EnvironmentVariableUtility.LoadEnv();
var dbPassword = EnvironmentVariableUtility.GetEnvironmentVariable("DB_PASSWORD");

var connectionString = EnvironmentVariableUtility.GetEnvironmentVariable("SQL_CONNECTION_STRING");

var jwtKey = EnvironmentVariableUtility.GetEnvironmentVariable("JWT_KEY");
builder.Configuration["Jwt:Key"] = jwtKey;
#endregion

builder.Services.AddDbContext<FootballLeagueDbContext>(options => 
{
    options.UseSqlServer(connectionString)
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
        //.LogTo(Console.WriteLine, LogLevel.Information)
        .ConfigureWarnings(warings => warings.Ignore(RelationalEventId.PendingModelChangesWarning));

    if (!builder.Environment.IsProduction())
    {
        options.EnableSensitiveDataLogging()
        .EnableDetailedErrors();
    }
});

builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<ICoachService, CoachService>();
builder.Services.AddScoped<ILeagueService, LeagueService>();
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
