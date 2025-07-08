using BankMore.Core.API.Extensions;
using BankMore.Core.API.Models;
using BankMore.Transfer.Application.UseCases.Transfer.Create;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace BankMore.Transfer.API.Controllers.V1;


[ApiController]
[ApiVersion("1.0")]
[Route("api/v1/transfer")]
public class TransferController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation("realizar transferencia de conta corrente")]
    [SwaggerResponse(StatusCodes.Status200OK, "Transferencia realizda com sucesso")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados inválidos", typeof(ApiErrorResponse))]
    public async Task<IActionResult> Create([FromBody] TransferCreateRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(request, cancellationToken);

        if (!result.IsSuccess)
            return result.ToHttpNonSuccessResult();

        return Ok(result.Value);
    }
}
