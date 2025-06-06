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

#region ConnectionStringConfiguration
EnvironmentVariableUtility.LoadEnv();
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD")
    ?? throw new InvalidOperationException("Environment variable DB_PASSWORD is not set.");

var connectionString = builder.Configuration.GetConnectionString("SqlServerConnectionString");
connectionString = connectionString?.Replace("{password}", dbPassword);
#endregion

builder.Services.AddDbContext<FootballLeagueDbContext>(options => 
{
    options.UseSqlServer(connectionString)
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
        .LogTo(Console.WriteLine, LogLevel.Information)
        .ConfigureWarnings(warings => warings.Ignore(RelationalEventId.PendingModelChangesWarning));

    if (!builder.Environment.IsProduction())
    {
        options.EnableSensitiveDataLogging()
        .EnableDetailedErrors();
    }
});

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
