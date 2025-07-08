using BankMore.Core.Domain.Abstracts;

namespace BankMore.Account.Domain.Entities;

public sealed class CheckingAccount : Entity
{
    public int NumberAccount { get; set; }

    public string? NationalDocument { get; set; }

    public string? Name { get; set; }

    public bool IsActive { get; set; }

    public string? Password { get; set; }

    public string? Salt { get; set; }

    public CheckingAccount() { }

    private CheckingAccount(int numberAccount, string? nationalDocument, string? name, string? password, string? salt)
    {
        NumberAccount = numberAccount;
        NationalDocument = nationalDocument;
        Name = name;
        Password = password;
        Salt = salt;
        IsActive = true;
    }

    public static CheckingAccount Create(string? nationalDocument, string? name, string? password, string? salt)
    {
        var random = new Random().Next(1000, 9999);

        return new CheckingAccount(random, nationalDocument, name, password, salt);
    }

    public void Inactivate() {
        IsActive = false;
    }
}
