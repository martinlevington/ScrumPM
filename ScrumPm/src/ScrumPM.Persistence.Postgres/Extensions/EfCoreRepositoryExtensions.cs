using System;
using Microsoft.EntityFrameworkCore;
using ScrumPm.Domain.Common.Persistence;

namespace ScrumPM.Persistence.Postgres.Extensions
{
    public static class EfCoreRepositoryExtensions
    {

        public static DbContext GetDbContext<TEntity, TKey>(this IReadOnlyBasicRepository<TEntity, TKey> repository)
            where TEntity : class, IDataEntity<TKey>
        {
            return repository.ToEfCoreRepository().DbContext;
        }

        public static DbSet<TEntity> GetDbSet<TEntity, TKey>(this IReadOnlyBasicRepository<TEntity, TKey> repository)
            where TEntity : class, IDataEntity<TKey>
        {
            return repository.ToEfCoreRepository().DbSet;
        }

        public static IEfCoreRepository<TEntity, TKey> ToEfCoreRepository<TEntity, TKey>(this IReadOnlyBasicRepository<TEntity, TKey> repository)
            where TEntity : class, IDataEntity<TKey>
        {
            var efCoreRepository = repository as IEfCoreRepository<TEntity, TKey>;
            if (efCoreRepository == null)
            {
                throw new ArgumentException("Given repository does not implement " + typeof(IEfCoreRepository<TEntity, TKey>).AssemblyQualifiedName, nameof(repository));
            }

            return efCoreRepository;
        }
    }
}
