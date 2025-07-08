namespace BankMore.Transfer.Domain.External.Account.Response;

public sealed class AccoutMovementResponseApi
{
    public int CheckingAccountId { get; set; }

    public int NumberAccount { get; set; }

    public char Type { get; set; }

    public decimal Value { get; set; }
}
