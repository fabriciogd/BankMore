using Microsoft.Extensions.Hosting;
using BankMore.Core.EventBus;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((context, services) =>
{
    var configuration = context.Configuration;
    services.AddEventBusContext(configuration);
});

var app = builder.Build();
app.UseEventBusContext();

app.Run();