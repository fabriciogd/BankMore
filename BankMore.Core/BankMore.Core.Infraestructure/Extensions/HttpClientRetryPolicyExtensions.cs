using Polly;
using Polly.Extensions.Http;

namespace BankMore.Core.Infraestructure.Extensions;

public static class HttpClientRetryPolicyExtensions
{
    private const int RetryAttempts = 3;
    private const int RetryAttemptsInterval = 3;

    public static async Task<HttpResponseMessage> PostWithRetryPolicyAsync(this HttpClient httpClient, string route, StringContent stringContent)
    {
        var retryPolicy = GetRetryPolicy();

        var context = new Context(route);

        return await retryPolicy.ExecuteAsync(x => httpClient.PostAsync(route, stringContent), context);
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(RetryAttempts, retryAttempt => TimeSpan.FromSeconds(RetryAttemptsInterval));
    }
}
