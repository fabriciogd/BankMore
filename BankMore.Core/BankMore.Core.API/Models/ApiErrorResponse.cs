﻿namespace BankMore.Core.API.Models;

public class ApiErrorResponse
{
    public ApiErrorResponse(string code, string description)
    {
        Code = code;
        Description = description;
    }

    public string Code { get; private set; }
    public string Description { get; private set; }
}
