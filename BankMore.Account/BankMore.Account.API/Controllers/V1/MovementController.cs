﻿using BankMore.Account.Application.UseCases.Account.Create;
using BankMore.Account.Application.UseCases.Movement.Balance;
using BankMore.Account.Application.UseCases.Movement.Create;
using BankMore.Core.API.Extensions;
using BankMore.Core.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace BankMore.Account.API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v1/movement")]
public class MovementController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation("Registrar novo movimento")]
    [SwaggerResponse(StatusCodes.Status200OK, "Movimento registrado com sucesso", typeof(AccountCreateResponse))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados inválidos", typeof(ApiErrorResponse))]
    public async Task<IActionResult> Create([FromBody] MovementCreateRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(request, cancellationToken);

        if (!result.IsSuccess)
            return result.ToHttpNonSuccessResult();

        return Ok(result.Value);
    }

    [HttpGet("balance")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation("Obter saldo")]
    [SwaggerResponse(StatusCodes.Status200OK, "Saldo retornado com sucesso", typeof(MovementBalanceResponse))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados inválidos", typeof(ApiErrorResponse))]
    public async Task<IActionResult> Balance(CancellationToken cancellationToken)
    {
        var request = new MovementBalanceRequest();

        var result = await mediator.Send(request, cancellationToken);

        if (!result.IsSuccess)
            return result.ToHttpNonSuccessResult();

        return Ok(result.Value);
    }
}
