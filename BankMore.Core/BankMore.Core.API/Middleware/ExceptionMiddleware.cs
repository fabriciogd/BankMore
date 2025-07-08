using BankMore.Core.API.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Security.Authentication;
using System.Text.Json;

namespace BankMore.Core.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (AuthenticationException ex)
        {
            await HandleAuthenticationExceptionAsync(context, ex);
        }
        catch(ValidationException ex)
        {
            await HandleValidationExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            await HandleGenericExceptionAsync(context);
        }
    }

    private static Task HandleAuthenticationExceptionAsync(HttpContext context, AuthenticationException exception)
    {
        var errorResponse = new ApiErrorResponse("INVALID_TOKEN", exception.Message);

        var result = JsonSerializer.Serialize(errorResponse);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

        return context.Response.WriteAsync(result);
    }

    private static Task HandleGenericExceptionAsync(HttpContext context)
    {
        var errorResponse = new ApiErrorResponse("UNEXCPECTED_ERROR", "Ocorreu um erro desconhecido");

        var result = JsonSerializer.Serialize(errorResponse);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        return context.Response.WriteAsync(result);
    }

    private static Task HandleValidationExceptionAsync(HttpContext context, ValidationException ex)
    {
        var firstError = ex.Errors.First();

        var errorResponse = new ApiErrorResponse(firstError.ErrorCode, firstError.ErrorMessage);

        var result = JsonSerializer.Serialize(errorResponse);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        return context.Response.WriteAsync(result);
    }
}
