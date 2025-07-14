using BankMore.Account.Application.UseCases.Authentication.Login;
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
[Route("api/v1/authentication")]
public class AuthenticationController(IMediator mediator) : BaseController
{
    [AllowAnonymous]
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation("Logar na conta corrente")]
    [SwaggerResponse(StatusCodes.Status200OK, "Conta corrente logada com sucesso", typeof(AuthenticationLoginResponse))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados inválidos", typeof(ApiErrorResponse))]
    public async Task<IActionResult> Login([FromBody] AuthenticationLoginRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(request, cancellationToken);

        return result.MatchToResult();
    }
}
