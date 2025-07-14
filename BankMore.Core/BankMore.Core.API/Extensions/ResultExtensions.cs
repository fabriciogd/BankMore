using BankMore.Core.API.Models;
using BankMore.Core.Domain.Enums;
using BankMore.Core.Domain.Primitives;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankMore.Core.API.Extensions;

public static class ResultExtensions
{
    public static IActionResult MatchToResult<TResult>(this Result<TResult> result,
            Func<TResult, IActionResult> successAction = null!)
    {
        {
            successAction ??= (r) => new OkObjectResult(r);

            return result.Match(
                success => successAction(success),
                failure => ToHttpNonSuccessResult(failure));
        }
    }

    public static IActionResult ToHttpNonSuccessResult(Error error)
    {
        var statusCode = error.ErrorType switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.AccessUnAuthorized => StatusCodes.Status401Unauthorized,
            ErrorType.AccessForbidden => StatusCodes.Status403Forbidden,
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };

        return new ObjectResult(
            new ApiErrorResponse(error.Code, error.Description))
            {
                StatusCode = statusCode
            };
    }
};
