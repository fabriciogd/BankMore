using BankMore.Core.Infraestructure.Contracts;

namespace BankMore.Core.Infraestructure.Models;

internal sealed class UserIdentity : IUserIdentity
{
    public int NumberAccount { get; set; }
}
