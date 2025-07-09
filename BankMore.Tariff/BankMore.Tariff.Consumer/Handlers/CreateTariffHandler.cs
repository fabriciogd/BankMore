using BankMore.Core.EventBus.Consumers;
using BankMore.Core.EventBus.Contracts;
using BankMore.Core.EventBus.Models;
using BankMore.Tariff.Consumer.Interfaces;
using BankMore.Tariff.Consumer.Models;
using Microsoft.Extensions.Configuration;

namespace BankMore.Tariff.Consumer.Handlers;

public sealed class CreateTariffHandler(ITariffRepository repository, IConfiguration configuration) : BaseConsumerHandler, IConsumer
{
    public string Name => nameof(CreateTariffHandler);

    public async Task Handle(Body message)
    {
        var value = configuration["Tariff"];

        var command = GetDeserializeBodyMessage<TransferEventRequest>(message);

        var tariff = new Domain.Tariff()
        {
            CheckingAccountId = command.CheckingAccountId,
            Date = DateTime.Now,
            Value = Convert.ToDecimal(value),
        };

        await repository.AddAsync(tariff, CancellationToken.None);
    }
}
