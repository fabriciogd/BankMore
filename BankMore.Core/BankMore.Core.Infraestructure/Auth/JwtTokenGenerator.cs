using BankMore.Core.Application.Iterfaces;
using BankMore.Core.Domain.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BankMore.Core.Infraestructure.Auth;

internal sealed class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IConfiguration _configuration;

    public JwtTokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(int numberAccount)
    {
        var secretKey = _configuration["JwtSettings:SecretKey"];

        var hashPassphrase = _configuration["RsaKeys:HashPassphrase"];

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, numberAccount.ToString().EncryptWithPassphrase(hashPassphrase))
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}