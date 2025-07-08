namespace BankMore.Account.Application.UseCases.Movement.Create;

public sealed record MovementCreateResponse
{
    public int CheckingAccountId { get; set; }

    public int NumberAccount { get; set; }

    public char Type { get; set; }

    public decimal Value { get; set; }
}
