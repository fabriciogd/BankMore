namespace BankMore.Account.Application.UseCases.Movement.Balance;

public sealed record MovementBalanceResponse
{
    public int NumberAccount { get; set; }

    public string? Name { get; set; }

    public DateTime Date { get; set; }

    public decimal Balance { get; set; }
}
