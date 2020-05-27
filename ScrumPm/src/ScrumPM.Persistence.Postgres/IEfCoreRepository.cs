using Microsoft.EntityFrameworkCore;
using ScrumPm.Domain.Common.Persistence;

namespace ScrumPM.Persistence.Postgres
{
    public interface IEfCoreRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IDataEntity
    {
        DbContext DbContext { get; }

        DbSet<TEntity> DbSet { get; }
    }

    public interface IEfCoreRepository<TEntity, TKey> : IEfCoreRepository<TEntity>, IRepository<TEntity, TKey>
        where TEntity : class, IDataEntity<TKey>
    {
    }
}