using System.Security.Cryptography;

namespace BankMore.Core.Infraestructure.Security;

public static class PasswordHasher
{
    private const int SaltSize = 16;
    private const int KeySize = 32;
    private const int Iterations = 100_000;

    public static string HashPassword(string plainPassword, out string salt)
    {
        byte[] saltBytes = new byte[SaltSize];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(saltBytes);

        using var pbkdf2 = new Rfc2898DeriveBytes(plainPassword, saltBytes, Iterations, HashAlgorithmName.SHA256);
        byte[] key = pbkdf2.GetBytes(KeySize);

        salt = Convert.ToBase64String(saltBytes);
        return Convert.ToBase64String(key);
    }

    public static bool VerifyPassword(string plainPassword, string hashedPassword, string salt)
    {
        byte[] saltBytes = Convert.FromBase64String(salt);
        byte[] storedHashBytes = Convert.FromBase64String(hashedPassword);

        using var pbkdf2 = new Rfc2898DeriveBytes(plainPassword, saltBytes, Iterations, HashAlgorithmName.SHA256);
        byte[] newKey = pbkdf2.GetBytes(KeySize);

        return CryptographicOperations.FixedTimeEquals(newKey, storedHashBytes);
    }
}
