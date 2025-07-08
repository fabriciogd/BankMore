using Microsoft.AspNetCore.Mvc;

namespace BankMore.Core.API.Attrubutes;

public class IdempotencyAttribute : TypeFilterAttribute
{
    public IdempotencyAttribute() : base(typeof(Filters.IdempotencyFilter)) { }
}
