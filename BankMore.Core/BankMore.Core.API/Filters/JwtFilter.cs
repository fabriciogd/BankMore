using BankMore.Core.Domain.Extensions;
using BankMore.Core.Infraestructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Authentication;
using System.Text;

namespace BankMore.Core.API.Filters;

public sealed class JwtFilter(IConfiguration configuration, IUserIdentity userIdentity) : IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context)
    {
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var actionDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;

        if (actionDescriptor.MethodInfo.GetCustomAttributes<AllowAnonymousAttribute>().Any())
            return;

        string jwtToken = GetBearerToken(context.HttpContext.Request);

        if (string.IsNullOrEmpty(jwtToken))
            throw new AuthenticationException("Token não identificado");

        var secretKey = configuration["JwtSettings:SecretKey"];

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        var tokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = key,
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateLifetime = true,
            RequireExpirationTime = true,
            RequireSignedTokens = true
        };

        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

        jwtSecurityTokenHandler.ValidateToken(jwtToken, tokenValidationParameters, out var tokenValidado);

        if (tokenValidado.ValidTo < DateTime.UtcNow)
            throw new AuthenticationException("Sua sessão expirou. Faça o login novamente.");

        userIdentity.NumberAccount = Convert.ToInt32(GetClaimFromJwtToken(jwtSecurityTokenHandler, jwtToken, JwtRegisteredClaimNames.Sub));
    }

    private string GetBearerToken(HttpRequest request)
    {
        var authHeader = request.Headers["Authorization"].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
            return string.Empty;

        return authHeader["Bearer ".Length..].Trim();
    }

    private string GetClaimFromJwtToken(JwtSecurityTokenHandler jwtSecurityTokenHandler, string jtwToken, string claimName)
    {
        var jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(jtwToken);
        return jwtSecurityToken.Claims.First(claim => claim.Type == claimName)
            .Value
            .DecryptWithPassphrase(configuration["RsaKeys:HashPassphrase"]);
    }
}
