using BankMore.Core.Domain.Abstracts;

namespace BankMore.Transfer.Domain.Entities;

public class Transfer : Entity
{
    public int SouceCheckingAccountId { get; set; }

    public int DestinationCheckingAccountId { get; set; }

    public DateTime Date { get; set; }

    public decimal Value { get; set; }

    public static Transfer Create(int sourceCheckingAccountId, int destinationCheckingAccountId, DateTime date, decimal value)
        => new()
        {
            SouceCheckingAccountId = sourceCheckingAccountId,
            DestinationCheckingAccountId = destinationCheckingAccountId,
            Date = date,
            Value = value
        };
}
