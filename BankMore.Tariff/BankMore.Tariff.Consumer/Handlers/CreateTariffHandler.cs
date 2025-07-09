using BankMore.Core.EventBus.Consumers;
using BankMore.Core.EventBus.Contracts;
using BankMore.Core.EventBus.Models;
using BankMore.Tariff.Consumer.Models;
using Microsoft.Extensions.Logging;

namespace BankMore.Tariff.Consumer.Handlers;

public sealed class CreateTariffHandler(ILogger<CreateTariffHandler> logger) : BaseConsumerHandler, IConsumer
{
    public string Name => nameof(CreateTariffHandler);

    public async Task Handle(Body message)
    {
        var command = GetDeserializeBodyMessage<TransferEventRequest>(message);

        logger.LogInformation("Transferencia realizada. Aplicar tarifa em {@Command}", command);
    }
}
