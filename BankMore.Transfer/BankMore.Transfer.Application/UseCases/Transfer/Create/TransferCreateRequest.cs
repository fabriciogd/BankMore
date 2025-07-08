using BankMore.Core.Domain.Primitives;
using MediatR;

namespace BankMore.Transfer.Application.UseCases.Transfer.Create;

public sealed record TransferCreateRequest : IRequest<Result<Unit>>
{
    public int DestinationNumberAccount { get; set; }
    
    public decimal Value { get; set; }
}
