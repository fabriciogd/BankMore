using BankMore.Core.EventBus.Models;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Serialization;
using Rebus.Serialization.Json;
using Rebus.Transport;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BankMore.Core.EventBus.Helpers;

public static class BusConfiguration
{
    public static RebusConfigurer GetConsumerConfig(IHandlerActivator handler, ConsumerSettings settings,
        Action<StandardConfigurer<ITransport>> transport)
    {
        return Configure.With(handler)
            .Serialization(GetSerializationOptions())
            .Transport(transport)
            .Options(GetConsumerConfigOptions(settings));
    }

    public static RebusConfigurer GetPublisherConfig(IHandlerActivator handler,
        Action<StandardConfigurer<ITransport>> transport)
    {
        return Configure.With(handler)
            .Serialization(GetSerializationOptions())
            .Transport(transport)
            .Options(GetPublisherConfigOptions());
    }

    private static Action<StandardConfigurer<ISerializer>> GetSerializationOptions()
    {
        return x => x.UseSystemTextJson(new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            IgnoreReadOnlyFields = true,
            IgnoreReadOnlyProperties = true
        });
    }

    private static Action<OptionsConfigurer> GetPublisherConfigOptions()
    {
        return o =>
        {
            o.Decorate<ISerializer>(c => new EventBusSerializer(c.Get<ISerializer>()));
        };
    }

    private static Action<OptionsConfigurer> GetConsumerConfigOptions(ConsumerSettings settings)
    {
        return o =>
        {
            o.Decorate<ISerializer>(c => new EventBusSerializer(c.Get<ISerializer>()));
        };
    }
}
