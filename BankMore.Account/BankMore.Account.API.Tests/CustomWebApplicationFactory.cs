using BankMore.Core.Application.Iterfaces;
using BankMore.Core.Infraestructure.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BankMore.Account.API.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public CustomWebApplicationFactory()
    {
        
    }
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(IDbConnectionFactory));

            services.TryAddSingleton<IDbConnectionFactory>(new SqliteConnectionFactory("Data Source=Teste.db;Mode=ReadWriteCreate;Cache=Shared;Pooling=false;"));
        });

        base.ConfigureWebHost(builder);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (File.Exists("Teste.db"))
            File.Delete("Teste.db");
    }
}
