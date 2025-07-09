using Microsoft.Extensions.Hosting;
using BankMore.Core.EventBus;
using BankMore.Core.Infraestructure;
using BankMore.Core.Application.Iterfaces;
using Microsoft.Extensions.DependencyInjection;
using BankMore.Tariff.Consumer;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((context, services) =>
{
    var configuration = context.Configuration;
    services.AddSqliteContext(configuration);
    services.AddEventBusContext(configuration);
    services.AddRepositories();
});

var app = builder.Build();
app.UseEventBusContext();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<IDbConnectionFactory>();
    await db.RunMigrationsAsync(AssemblyReference.Assembly);
}

app.Run();