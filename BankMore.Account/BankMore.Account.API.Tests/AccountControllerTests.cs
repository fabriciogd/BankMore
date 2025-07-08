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
        //Arrange
        var request = new AccountCreateRequest()
        {
            Name = "Teste",
            NationalDocument = "69808125050",
            Password = "123"
        };

        var content = JsonContent.Create(request);
        content.Headers.Add("Idempotency-Key", "111");

        //Act
        var response = await _httpClient
            .PostAsync("api/v1/account", content);

        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Should_Return_Success_When_Request_Idempotent()
    {
        //Arrange
        var request = new AccountCreateRequest()
        {
            Name = "Teste",
            NationalDocument = "81215896069",
            Password = "123"
        };

        var content = JsonContent.Create(request);
        content.Headers.Add("Idempotency-Key", "222");

        //Act
        var response = await _httpClient
            .PostAsync("api/v1/account", content);

        response = await _httpClient
            .PostAsync("api/v1/account", content);

        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(response.Headers.Contains("Idempotency-Cache"));
    }


    [Fact]
    public async Task Should_Return_Error_When_Request_Invalid()
    {
        //Arrange
        var request = new AccountCreateRequest()
        {
            Name = "Teste",
            NationalDocument = "69808125053",
            Password = "123"
        };

        var content = JsonContent.Create(request);
        content.Headers.Add("Idempotency-Key", "333");

        //Act
        var response = await _httpClient
            .PostAsync("api/v1/account", content);

        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
