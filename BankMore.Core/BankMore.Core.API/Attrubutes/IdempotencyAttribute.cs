using BankMore.Core.API.Filters;
using Microsoft.AspNetCore.Mvc;

namespace BankMore.Core.API.Attrubutes;

public class IdempotencyAttribute : TypeFilterAttribute
{
    public IdempotencyAttribute() : base(typeof(IdempotencyFilter)) { }
}
