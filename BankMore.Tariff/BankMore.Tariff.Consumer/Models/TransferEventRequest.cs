namespace BankMore.Tariff.Consumer.Models;

public class TransferEventRequest
{
    public int CheckingAccountId { get; set; }

    public DateTime Date { get; set; }

    public decimal Value { get; set; }
}
