using BankMore.Core.Domain.Abstracts;

namespace BankMore.Tariff.Consumer.Domain;

public class Tariff : Entity
{
    public int CheckingAccountId { get; set; }

    public DateTime Date { get; set; }

    public decimal Value { get; set; }
}
