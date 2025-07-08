using System.Security.Cryptography;
using System.Text;

namespace BankMore.Core.Domain.Extensions;

public static class CryptographyExtensions
{
    public static string EncryptWithPassphrase(this string plainText, string passPhrase)
    {
        const int byteSize = 16;

        if (plainText == null)
            return null;

        var plainTextBytes = Encoding.UTF8.GetBytes(plainText);

        using var password = new Rfc2898DeriveBytes(passPhrase, new byte[byteSize], 100000, HashAlgorithmName.SHA512);
        var keyBytes = password.GetBytes(byteSize);

        using var symmetricKey = Aes.Create();
        symmetricKey.Mode = CipherMode.CBC;

        using var encryptor = symmetricKey.CreateEncryptor(keyBytes, new byte[byteSize]);
        using var memoryStream = new MemoryStream();

        using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
        cryptoStream.FlushFinalBlock();

        var cipherTextBytes = memoryStream.ToArray();

        return Convert.ToBase64String(cipherTextBytes);
    }

    public static string DecryptWithPassphrase(this string cryptedText, string passPhrase)
    {
        const int byteSize = 16;

        if (string.IsNullOrEmpty(cryptedText))
            return null;

        var cipherTextBytes = Convert.FromBase64String(cryptedText);

        using var password = new Rfc2898DeriveBytes(passPhrase, new byte[byteSize], 100000, HashAlgorithmName.SHA512);
        var keyBytes = password.GetBytes(byteSize);
        using var symmetricKey = Aes.Create();
        symmetricKey.Mode = CipherMode.CBC;

        using var decryptor = symmetricKey.CreateDecryptor(keyBytes, new byte[byteSize]);
        using var memoryStream = new MemoryStream(cipherTextBytes);
        using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        var plainTextBytes = new byte[cipherTextBytes.Length];
        var totalReadCount = 0;

        while (totalReadCount < cipherTextBytes.Length)
        {
            var buffer = new byte[cipherTextBytes.Length];
            var readCount = cryptoStream.Read(buffer, 0, buffer.Length);
            if (readCount == 0) break;

            for (var i = 0; i < readCount; i++)
                plainTextBytes[i + totalReadCount] = buffer[i];

            totalReadCount += readCount;
        }

        return Encoding.UTF8.GetString(plainTextBytes, 0, totalReadCount);
    }
}
