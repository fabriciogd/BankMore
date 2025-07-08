namespace BankMore.Transfer.Domain.External.Account.Request;

public sealed record AccountMovementRequestApi
{
    public int NumberAccount { get; set; }

    public char Type { get; set; }

    public decimal Value { get; set; }
}
