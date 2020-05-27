using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ScrumPm.Domain.Common.Extensions;
using ScrumPm.Domain.Common.Persistence;
using ScrumPm.Domain.Common.Threading;

namespace ScrumPM.Persistence.Postgres
{
    public class EfCoreRepository<TDbContext, TEntity, TKey>
        where TDbContext : DbContext, IEfCoreDbContext
        where TEntity : class, IDataEntity<TKey>
    {
        private readonly IEfCoreDbContext _dbContext;
        private readonly IDbContextProvider<TDbContext> _dbContextProvider;

        protected EfCoreRepository(IDbContextProvider<TDbContext> dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
            _dbContext = _dbContextProvider.GetDbContext();

            CancellationTokenProvider = NullCancellationTokenProvider.Instance;
        }

        public ICancellationTokenProvider CancellationTokenProvider { get; set; }

        private DbSet<TEntity> Set => DbContext.Set<TEntity>();

        protected virtual TDbContext DbContext => _dbContextProvider.GetDbContext();
        public IQueryable<TEntity> Queryable => Set.AsNoTracking();


        public DbSet<TEntity> DbSet { get; }

        public TEntity Insert(TEntity entity, bool autoSave = false)
        {
            var savedEntity = DbSet.Add(entity).Entity;

            if (autoSave)
            {
                DbContext.SaveChanges();
            }

            return savedEntity;
        }

        public async Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false,
            CancellationToken cancellationToken = default)
        {
            var savedEntity = DbSet.Add(entity).Entity;

            if (autoSave)
            {
                await DbContext.SaveChangesAsync(GetCancellationToken(cancellationToken));
            }

            return savedEntity;
        }

        protected virtual CancellationToken GetCancellationToken(CancellationToken preferredValue = default)
        {
            return CancellationTokenProvider.FallbackToProvider(preferredValue);
        }

        public TEntity Update(TEntity entity, bool autoSave = false)
        {
            DbContext.Attach(entity);

            var updatedEntity = DbContext.Update(entity).Entity;

            if (autoSave)
            {
                DbContext.SaveChanges();
            }

            return updatedEntity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false,
            CancellationToken cancellationToken = default)
        {
            DbContext.Attach(entity);

            var updatedEntity = DbContext.Update(entity).Entity;

            if (autoSave)
            {
                await DbContext.SaveChangesAsync(cancellationToken);
            }

            return updatedEntity;
        }

        public void Delete(TEntity entity, bool autoSave = false)
        {
            DbSet.Remove(entity);

            if (autoSave)
            {
                DbContext.SaveChanges();
            }
        }

        public async Task DeleteAsync(TEntity entity, bool autoSave = false,
            CancellationToken cancellationToken = default)
        {
            DbSet.Remove(entity);

            if (autoSave)
            {
                await DbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public List<TEntity> GetList(bool includeDetails = false)
        {
            return includeDetails
                ? WithDetails().ToList()
                : DbSet.ToList();
        }

        public async Task<List<TEntity>> GetListAsync(bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return includeDetails
                ? await WithDetails().ToListAsync(cancellationToken)
                : await DbSet.ToListAsync(cancellationToken);
        }

        public void Delete(Expression<Func<TEntity, bool>> predicate, bool autoSave = false)
        {
            Delete(predicate, autoSave);

            if (autoSave)
            {
                DbContext.SaveChanges();
            }
        }

        public async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, bool autoSave = false,
            CancellationToken cancellationToken = default)
        {
            var entities = await GetQueryable().Where(predicate).ToListAsync(GetCancellationToken(cancellationToken));
            foreach (var entity in entities)
            {
                DbSet.Remove(entity);
            }

            if (autoSave)
            {
                await DbContext.SaveChangesAsync(cancellationToken);
            }
        }


        public IQueryable<TEntity> WithDetails(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            var query = GetQueryable();

            if (!propertySelectors.IsNullOrEmpty())
            {
                foreach (var propertySelector in propertySelectors)
                {
                    query = query.Include(propertySelector);
                }
            }

            return query;
        }

        public long GetCount()
        {
            return DbSet.LongCount();
        }

        public async Task<long> GetCountAsync(CancellationToken cancellationToken = default)
        {
            return await DbSet.LongCountAsync(cancellationToken);
        }

        protected IQueryable<TEntity> GetQueryable()
        {
            return DbSet.AsQueryable();
        }

        public virtual async Task EnsureCollectionLoadedAsync<TProperty>(
            TEntity entity,
            Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression,
            CancellationToken cancellationToken = default)
            where TProperty : class
        {
            await DbContext.Entry(entity).Collection(propertyExpression)
                .LoadAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task EnsurePropertyLoadedAsync<TProperty>(
            TEntity entity,
            Expression<Func<TEntity, TProperty>> propertyExpression,
            CancellationToken cancellationToken = default)
            where TProperty : class
        {
            await DbContext.Entry(entity).Reference(propertyExpression)
                .LoadAsync(GetCancellationToken(cancellationToken));
        }
    }
}