using BankMore.Core.Domain.Primitives;
using MediatR;

namespace BankMore.Account.Application.UseCases.Movement.Create;

public class MovementCreateRequest : IRequest<Result<MovementCreateResponse>>
{
    public int? NumberAccount { get; set; }

    public char Type { get; set; }

    public decimal Value { get; set; }
}
