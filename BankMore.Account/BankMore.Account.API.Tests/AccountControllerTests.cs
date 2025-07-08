using BankMore.Account.Application.UseCases.Account.Create;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace BankMore.Account.API.Tests;

public class AccountControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;

    private JsonSerializerOptions _options => new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public AccountControllerTests(CustomWebApplicationFactory appFactory)
    {
        _httpClient = appFactory.CreateClient();
    }

    [Fact]
    public async Task Should_Return_Success_When_Request_Correct()
    {
        var request = new AccountCreateRequest()
        {
            Name = "Teste",
            NationalDocument = "69808125050",
            Password = "123"
        };

        var response = await _httpClient
            .PostAsJsonAsync("api/v1/account", request, _options);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Should_Return_Error_When_Request_Invalid()
    {
        var request = new AccountCreateRequest()
        {
            Name = "Teste",
            NationalDocument = "69808125053",
            Password = "123"
        };

        var response = await _httpClient
            .PostAsJsonAsync("api/v1/account", request, _options);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
