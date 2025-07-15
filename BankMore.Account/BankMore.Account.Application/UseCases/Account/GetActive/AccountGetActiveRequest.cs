using BankMore.Core.Domain.Primitives;
using MediatR;

namespace BankMore.Account.Application.UseCases.Account.GetActive;

public sealed record AccountGetActiveRequest(int NumberAccount) : IRequest<Result<AccountGetActiveResponse>>;