using BankMore.Core.Domain.Primitives;
using MediatR;

namespace BankMore.Account.Application.UseCases.Account.Inactivate;

public sealed record AccountInactivateRequest : IRequest<Result<Unit>>;
