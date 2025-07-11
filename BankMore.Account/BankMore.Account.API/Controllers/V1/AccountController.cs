﻿using BankMore.Account.Application.UseCases.Account.Create;
using BankMore.Account.Application.UseCases.Account.Inactivate;
using BankMore.Core.API.Attrubutes;
using BankMore.Core.API.Base;
using BankMore.Core.API.Extensions;
using BankMore.Core.API.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace BankMore.Account.API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v1/account")]
public class AccountController(IMediator mediator): BaseController
{
    [Idempotency]
    [AllowAnonymous]
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation("Cadastrar conta corrente")]
    [SwaggerResponse(StatusCodes.Status200OK, "Conta corrente cadastrada com sucesso", typeof(AccountCreateResponse))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados inválidos", typeof(ApiErrorResponse))]
    public async Task<IActionResult> Create([FromBody] AccountCreateRequest request, CancellationToken cancellationToken)
    {
        await ValidateAndThrowAsync<AccountCreateRequestValidator, AccountCreateRequest>(request);

        var result = await mediator.Send(request, cancellationToken);

        if (!result.IsSuccess)
            return result.ToHttpNonSuccessResult();

        return Ok(result.Value);
    }

    [HttpPut]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation("Inativar conta corrente")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Conta corrente inativada com sucesso")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados inválidos", typeof(ApiErrorResponse))]
    public async Task<IActionResult> Inactivate(CancellationToken cancellationToken)
    {
        var request = new AccountInactivateRequest();

        var result = await mediator.Send(request, cancellationToken);

        if (!result.IsSuccess)
            return result.ToHttpNonSuccessResult();

        return Ok();
    }
}
