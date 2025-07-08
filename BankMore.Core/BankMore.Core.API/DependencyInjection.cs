using BankMore.Core.API.Filters;
using BankMore.Core.Infraestructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;

namespace BankMore.Core.API;

public static class DependencyInjection
{
    public static IServiceCollection AddIdempotency(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IdempotencyFilter>();

        services.AddSingleton<IConnectionMultiplexer>(
            _ => ConnectionMultiplexer.Connect(configuration["Redis:Url"])
        );

        services.AddIdempotencyService();

        return services;
    }

    public static IServiceCollection AddBearerAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        var secretKey = configuration["JwtSettings:SecretKey"];

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            };
        });

        return services;
    }
}
