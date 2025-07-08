using System.Data;
using System.Reflection;

namespace BankMore.Core.Application.Iterfaces;

public interface IDbConnectionFactory
{
    Task<IDbConnection> CreateOpenConnectionAsync();

    Task RunMigrationsAsync(Assembly assembly);
}
