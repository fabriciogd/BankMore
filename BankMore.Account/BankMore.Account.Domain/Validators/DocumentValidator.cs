namespace BankMore.Account.Domain.Validators;

public static class DocumentValidator
{
    public static bool IsValid(string? cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return false;

        cpf = new string(cpf.Where(char.IsDigit).ToArray());

        if (cpf.Length != 11)
            return false;

        var invalidCpfs = new[]
        {
            "00000000000", "11111111111", "22222222222", "33333333333",
            "44444444444", "55555555555", "66666666666", "77777777777",
            "88888888888", "99999999999"
        };

        if (invalidCpfs.Contains(cpf))
            return false;

        var digits = cpf.Select(c => int.Parse(c.ToString())).ToArray();

        int sum = 0;
        for (int i = 0; i < 9; i++)
            sum += digits[i] * (10 - i);

        int firstDigit = sum % 11;
        firstDigit = firstDigit < 2 ? 0 : 11 - firstDigit;

        if (digits[9] != firstDigit)
            return false;

        sum = 0;
        for (int i = 0; i < 10; i++)
            sum += digits[i] * (11 - i);

        int secondDigit = sum % 11;
        secondDigit = secondDigit < 2 ? 0 : 11 - secondDigit;

        return digits[10] == secondDigit;
    }
}
