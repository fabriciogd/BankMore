using BankMore.Core.Domain.Enums;
using BankMore.Core.Domain.Primitives;
using BankMore.Core.Infraestructure.Extensions;
using BankMore.Core.Infraestructure.External.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;
using System.Text;

namespace BankMore.Core.Infraestructure.External;

public abstract class BaseApiService(HttpClient client)
{
    private static StringContent GetStringContent(object obj)
    {
        return new StringContent(JsonConvert.SerializeObject(obj,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore
                }),
            Encoding.UTF8, "application/json");
    }

    protected static T GetResultContentAsync<T>(string content) => JsonConvert.DeserializeObject<T>(content);

    protected async Task<Result<TResponse>> PostAsync<TRequest, TResponse, TErrorResponse>(string resource, TRequest request)
        where TRequest : class
        where TResponse : class
        where TErrorResponse: IErrorResponse
    {
        var startRequest = DateTime.Now;
        var endpoint = $"{client.BaseAddress}{resource}";

        var httpResponse = await client.PostWithRetryPolicyAsync(endpoint, GetStringContent(request));

        return await ProcessResultContentAsync<TResponse, TErrorResponse>(httpResponse);
    }

    private async Task<Result<TResponse>> ProcessResultContentAsync<TResponse, TErrorResponse>(
        HttpResponseMessage httpResponse)
        where TResponse: class
        where TErrorResponse: IErrorResponse
    {
        var content = await httpResponse.Content.ReadAsStringAsync();

        if (httpResponse.IsSuccessStatusCode)
        {
            var resultResponse = GetResultContentAsync<TResponse>(content);

            return resultResponse;
        }

        if (httpResponse.StatusCode == HttpStatusCode.Forbidden)
        {
            return Error.AccessForbidden("ACCESS_FORBIDDEN", "Falha ao enviar requisição");
        }

        var errorResponse = GetResultContentAsync<TErrorResponse>(content);

        var errorType = httpResponse.StatusCode switch
        {
            HttpStatusCode.NotFound => ErrorType.NotFound,
            HttpStatusCode.BadRequest => ErrorType.Validation,
            HttpStatusCode.Conflict => ErrorType.Conflict,
            HttpStatusCode.Unauthorized => ErrorType.AccessUnAuthorized,
            HttpStatusCode.Forbidden => ErrorType.AccessForbidden,
            HttpStatusCode.InternalServerError => ErrorType.Failure,
            _ => ErrorType.Failure
        };

        return Error.Create(errorResponse.GetErrorCode(), errorResponse.GetErrorDescription(), errorType);
    }


}
