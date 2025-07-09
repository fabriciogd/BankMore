using BankMore.Core.EventBus.Contracts;
using BankMore.Core.EventBus.Models;
using Rebus.Bus;
using Rebus.Config;

namespace BankMore.Core.EventBus.Implementation;

internal sealed class PublisherEndpoint : IPublisherEndpoint
{
    private readonly IBus _bus;

    public PublisherEndpoint(RebusConfigurer rebusConfig)
    {
        _bus = rebusConfig.Start();
    }

    public Task PublishAsync(string topic, Body body, CancellationToken cancellationToken = default)
    {
        return _bus.Advanced.Topics.Publish(topic, body);
    }
}
