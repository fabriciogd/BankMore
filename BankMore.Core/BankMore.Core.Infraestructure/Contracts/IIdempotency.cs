namespace BankMore.Core.Infraestructure.Contracts;

public interface IIdempotency
{
    string Key { get; set; }
}
