using BankMore.Core.EventBus.Builders;
using BankMore.Core.EventBus.Contracts;
using BankMore.Core.EventBus.Helpers;
using BankMore.Core.EventBus.Implementation;
using BankMore.Core.EventBus.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Transport;
using System.Reflection;

namespace BankMore.Core.EventBus;

public static class Bootstrapper
{
    public static IServiceCollection AddEventBusContext(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = Load(configuration);

        services.AddSingleton(settings);

        var transport = GetTransport(settings);

        return services
              .AddSingleton(BusConfiguration.GetPublisherConfig(new BuiltinHandlerActivator(), transport))
              .AddSingleton<IPublisherEndpoint, PublisherEndpoint>()
              .AddTransient(_ => new BuiltinHandlerActivator())
              .AddSingletonHandler(settings);
    }

    public static IHost UseEventBusContext(this IHost hostBuilder)
    {
        var consumers = hostBuilder.Services.GetServices<IConsumer>();
        var settings = hostBuilder.Services.GetRequiredService<EventBusSettings>();

        consumers.AsParallel().ForAll(InitializeConsumer(hostBuilder, settings));

        return hostBuilder;
    }

    private static Action<IConsumer> InitializeConsumer(IHost hostBuilder, EventBusSettings settings)
    {
        return consumer =>
        {
            var handler = hostBuilder.RegisterEventBusConsumer(consumer);
            var builder = new ConsumerBuilder(handler);
            var consumerSettings = settings.Consumers.SingleOrDefault(x => x.Name == consumer.Name);
            if (consumerSettings is null) return;
            builder.Build(consumerSettings, GetTransport(settings, consumerSettings.Queue, consumerSettings.Prefetch));
        };
    }

    public static BuiltinHandlerActivator RegisterEventBusConsumer(this IHost hostBuilder, IConsumer consumer)
    {
        var handler = hostBuilder.Services.GetRequiredService<BuiltinHandlerActivator>();
        handler.Register(() => consumer);
        return handler;
    }

    private static IServiceCollection AddSingletonHandler(this IServiceCollection services,
        EventBusSettings settings)
    {
        var typesFromAssemblies = GetAssembliesFromConsumer();

        foreach (var type in typesFromAssemblies)
            services.AddSingleton(typeof(IConsumer), type);

        return services;
    }

    private static IEnumerable<TypeInfo> GetAssembliesFromConsumer()
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.DefinedTypes.Where(x => x.GetInterfaces().Contains(typeof(IConsumer))));
    }

    private static Action<StandardConfigurer<ITransport>> GetTransport(EventBusSettings settings,
        string? queueName = null, int? prefetch = null)
    {
        var machineName = Environment.MachineName;

        return transport => transport
            .UseRabbitMq(settings.ConnectionString, queueName ?? settings.DefaultQueue)
            .Declarations()
            .ExchangeNames("amq.direct", "amq.topic")
            .ClientConnectionName(machineName)
            .InputQueueOptions(opt => opt
                .SetDurable(false)
                .SetAutoDelete(false)
                .SetExclusive(false))
            .Prefetch(settings.DefaultPrefetch);
    }

    public static EventBusSettings? Load(IConfiguration configuration)
    {
        return configuration.GetSection(EventBusSettings.SessionName).Get<EventBusSettings>();
    }
}