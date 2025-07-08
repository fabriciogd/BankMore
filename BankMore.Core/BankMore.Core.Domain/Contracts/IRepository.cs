using BankMore.Core.Domain.Abstracts;

namespace BankMore.Core.Domain.Contracts;

public interface IRepository<TEntity> where TEntity : Entity
{
    Task<int> AddAsync(TEntity entity, CancellationToken cancellationToken);

    Task UpdateAsync(TEntity entity);

    Task RemoveAsync(TEntity entity);

    Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken);
}