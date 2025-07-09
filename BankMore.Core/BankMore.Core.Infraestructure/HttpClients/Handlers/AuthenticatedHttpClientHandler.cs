using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace BankMore.Core.Infraestructure.HttpClients.Handlers;

public class AuthenticatedHttpClientHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthenticatedHttpClientHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var accessToken = _httpContextAccessor.HttpContext?.Request?.Headers["Authorization"].ToString();

        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.Replace("Bearer ", ""));
        }

        return await base.SendAsync(request, cancellationToken);
    }
}