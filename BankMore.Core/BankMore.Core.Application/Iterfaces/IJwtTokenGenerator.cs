namespace BankMore.Core.Application.Iterfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(int numberAccount);
}
