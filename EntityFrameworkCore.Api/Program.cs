using EntityFrameworkCore.Application.Interfaces;
using EntityFrameworkCore.Application.Services;
using EntityFrameworkCore.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EntityFrameworkCore.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        #region EnvironmentVariableConfiguration
        string? connectionString = string.Empty;
        if (builder.Environment.IsDevelopment())
        {
            DotNetEnv.Env.TraversePath().Load();

            connectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");

            var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
            builder.Configuration["Jwt:Key"] = jwtKey;
        }
        #endregion

        builder.Services.AddDbContext<FootballLeagueDbContext>(options =>
        {
            options.UseNpgsql(connectionString)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .ConfigureWarnings(warings => warings.Ignore(RelationalEventId.PendingModelChangesWarning));

            if (!builder.Environment.IsProduction())
            {
                options.EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            }
        });

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                };
            });

        builder.Services.AddScoped<ITeamService, TeamService>();
        builder.Services.AddScoped<ICoachService, CoachService>();
        builder.Services.AddScoped<ILeagueService, LeagueService>();
        builder.Services.AddScoped<IMatchService, MatchService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IRoleService, RoleService>();

        // Register MediatR
        builder.Services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblies(
                typeof(EntityFrameworkCore.Application.Matches.Queries.GetMatchByIdQuery).Assembly
            ));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}

