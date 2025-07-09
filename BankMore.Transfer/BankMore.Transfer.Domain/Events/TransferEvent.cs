namespace BankMore.Transfer.Domain.Events;

public class TransferEvent
{
    public int CheckingAccountId { get; set; }

    public DateTime Date { get; set; }

    public decimal Value { get; set; }
}
