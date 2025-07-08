using BankMore.Core.Application.Services;
using BankMore.Core.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace BankMore.Core.API.Filters;

public class IdempotencyFilter(IIdempotencyService _idempotencyService) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue("Idempotency-Key", out var key))
        {
            context.Result = new BadRequestObjectResult("Chave de idempotência não encontrada");
            return;
        }

        var entity = await _idempotencyService.TryGetAsync(key!);

        if (entity is not null)
        {
            context.Result = new ContentResult
            {
                StatusCode = entity.StatusCode,
                Content = entity.ResponseBody,
                ContentType = "application/json"
            };
            return;
        }

        var executedContext = await next();

        if (executedContext.Result is ObjectResult objectResult)
        {
            var serializedBody = JsonSerializer.Serialize(objectResult.Value);

            entity = new Idempotency()
            {
                Key = key,
                StatusCode = objectResult.StatusCode ?? 200,
                CreatedAt = DateTime.Now,
                ResponseBody = serializedBody
            };

            await _idempotencyService.SaveAsync(entity);
        }
    }
}
