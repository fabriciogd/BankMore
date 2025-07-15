using Polly;
using Polly.Extensions.Http;

namespace BankMore.Core.Infraestructure.Extensions;

public static class HttpClientRetryPolicyExtensions
{
    private const int RetryAttempts = 3;
    private const int RetryAttemptsInterval = 3;

    public static async Task<HttpResponseMessage> SendWithRetryPolicyAsync(
        this HttpClient httpClient, 
        HttpRequestMessage request, 
        CancellationToken cancellationToken)
    {
        var retryPolicy = GetRetryPolicy();

        var context = new Context(request.RequestUri.ToString());

        return await retryPolicy.ExecuteAsync(x => httpClient.SendAsync(request, cancellationToken), context);
    }

    public static async Task<HttpResponseMessage> GetWithRetryPolicyAsync(
        this HttpClient httpClient, 
        Uri uriRoute,
        CancellationToken cancellationToken)
    {
        var retryPolicy = GetRetryPolicy();

        var context = new Context(uriRoute.AbsolutePath);

        return await retryPolicy.ExecuteAsync(x => httpClient.GetAsync(uriRoute, cancellationToken), context);
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(RetryAttempts, retryAttempt => TimeSpan.FromSeconds(RetryAttemptsInterval));
    }
}
