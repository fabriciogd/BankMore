using BankMore.Core.Infraestructure.Constants;
using DbUp;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace BankMore.Core.API.Helpers;

public static class DbUpMigrator
{
    public static void RunMigrations(IConfiguration configuration, Assembly assembly)
    {
        string connectionString = configuration.GetConnectionString(ConnectionString.Default)!;

        var upgrader = DeployChanges.To
            .SqliteDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(assembly)
            .Build();

        upgrader.PerformUpgrade();
    }
}
