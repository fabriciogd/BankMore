using BankMore.Core.EventBus.Models;

namespace BankMore.Core.EventBus.Contracts;

public interface IPublisherEndpoint
{
    Task PublishAsync(string topic, Body body, CancellationToken cancellationToken = default);
}
