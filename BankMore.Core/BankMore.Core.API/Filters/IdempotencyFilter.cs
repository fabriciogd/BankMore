using BankMore.Core.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace BankMore.Core.API.Filters;

public class IdempotencyFilter(IIdempotencyService idempotencyService) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue("Idempotency-Key", out var key))
        {
            context.Result = new BadRequestObjectResult("Chave de idempotência não encontrada");
            return;
        }

        var (exists, statusCode, body) = await idempotencyService.TryGetAsync(key!);

        if (exists)
        {
            context.Result = new ContentResult
            {
                StatusCode = statusCode,
                Content = body,
                ContentType = "application/json"
            };
            return;
        }

        var executedContext = await next();

        if (executedContext.Result is ObjectResult objectResult)
        {
            var serializedBody = JsonSerializer.Serialize(objectResult.Value);

            await idempotencyService.SaveAsync(key!, objectResult.StatusCode ?? 200, serializedBody);
        }
    }
}
