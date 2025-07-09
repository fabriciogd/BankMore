using BankMore.Core.EventBus.Helpers;
using BankMore.Core.EventBus.Models;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Messages;
using Rebus.Transport;

namespace BankMore.Core.EventBus.Builders;

public class ConsumerBuilder(BuiltinHandlerActivator handler)
{
    public void Build(ConsumerSettings settings, Action<StandardConfigurer<ITransport>> transport)
    {
        var configure = BusConfiguration.GetConsumerConfig(handler, settings, transport);
        configure.Start();

        if (string.IsNullOrEmpty(settings.Topic))
            handler.Bus.Subscribe<Message>();

        handler.Bus.Advanced.Topics.Subscribe(settings.Topic);
    }
}
