using BankMore.Core.Domain.Abstracts;
using BankMore.Transfer.Domain.Constants;

namespace BankMore.Transfer.Domain.Entities;

public class Transfer : Entity
{
    public int SouceCheckingAccountId { get; set; }

    public int DestinationCheckingAccountId { get; set; }

    public DateTime Date { get; set; }

    public decimal Value { get; set; }

    public char Status { get; set; }

    public static Transfer Create(int sourceCheckingAccountId, int destinationCheckingAccountId, decimal value)
        => new()
        {
            SouceCheckingAccountId = sourceCheckingAccountId,
            DestinationCheckingAccountId = destinationCheckingAccountId,
            Date = DateTime.Now,
            Value = value,
            Status = TransferStatusType.Started
        };

    public void SetStatus(char status) => Status = status;
}
