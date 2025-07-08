using BankMore.Account.Application.UseCases.Account.Create;
using BankMore.Account.Domain.Entities;
using BankMore.Account.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace BankMore.Account.Application.Tests;

public class CreateAccountRequestHandlerTests
{
    private readonly Mock<ICheckingAccountRepository> _repository;
    private readonly Mock<ILogger<AccountCreateRequestHandler>> _logger;

    private readonly AccountCreateRequestHandler _handler;

    public CreateAccountRequestHandlerTests()
    {
        _repository = new Mock<ICheckingAccountRepository>();
        _logger = new Mock<ILogger<AccountCreateRequestHandler>>();
        _handler = new AccountCreateRequestHandler(_repository.Object, _logger.Object);
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

        _repository
            .Setup(x => x.AddAsync(It.Is<CheckingAccount>(x => 
                    x.Name == request.Name && 
                    x.NationalDocument == request.NationalDocument
                ), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(1);

        //Act
        var result = await _handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.True(result.IsSuccess);

        _repository.Verify(mock =>
            mock.AddAsync(It.Is<CheckingAccount>(x =>
                x.Name == request.Name && 
                x.NationalDocument == request.NationalDocument
                ), It.IsAny<CancellationToken>()
            ),
            Times.Once()
        );
    }
}
