using BankMore.Core.Domain.Primitives;
using MediatR;

namespace BankMore.Account.Application.UseCases.Movement.Balance;

public sealed record MovementBalanceRequest: IRequest<Result<MovementBalanceResponse>>;
