using BankMore.Core.Domain.Abstracts;

namespace BankMore.Account.Domain.Entities;

public sealed class MovementAccount : Entity
{
    public int CheckingAccountId { get; set; }

    public DateTime Date { get; set; }

    public char Type { get; set; }

    public decimal Value { get; set; }

    private MovementAccount(int checkingAccountId, DateTime date, char type, decimal value)
    {
        CheckingAccountId = checkingAccountId;
        Date = date;
        Type = type;
        Value = value;
    }

    public static MovementAccount Create(int checkingAccountId, char type, decimal value)
    {
        return new MovementAccount(checkingAccountId, DateTime.Now, type, value);
    }
}
