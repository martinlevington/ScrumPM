using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ScrumPm.Domain.Common.Extensions;
using ScrumPm.Domain.Common.Persistence;
using ScrumPm.Persistence.Common;
using ScrumPm.Persistence.EntityFrameworkCore;

namespace ScrumPm.Persistence
{
    public  class EfCoreRepository<TDbContext, TEntity, TKey> : RepositoryBase<TEntity, TKey>, IEfCoreRepository<TEntity>
        where TDbContext : IEfCoreDbContext
        where TEntity : class, IDataEntity<TKey>
    {
        private DbSet<TEntity> Set => DbContext.Set<TEntity>();

        DbContext IEfCoreRepository<TEntity>.DbContext => DbContext.As<DbContext>();
        

        protected virtual TDbContext DbContext => _dbContextProvider.GetDbContext();

        public DbSet<TEntity> DbSet { get; }
        public IQueryable<TEntity> Queryable => Set.AsNoTracking();

        protected virtual EntityOptions<TEntity> EntityOptions => _entityOptionsLazy.Value;

        private readonly IDbContextProvider<TDbContext> _dbContextProvider;
        private readonly Lazy<EntityOptions<TEntity>> _entityOptionsLazy;

        protected EfCoreRepository(IDbContextProvider<TDbContext> dbContextProvider)
        {

            _dbContextProvider = dbContextProvider;

            _entityOptionsLazy = new Lazy<EntityOptions<TEntity>>(
                () => ServiceProvider
                          .GetRequiredService<IOptions<EntityOptions>>()
                          .Value
                          .GetOrNull<TEntity>() ?? EntityOptions<TEntity>.Empty
            );

          
           
        }

         public override TEntity Insert(TEntity entity, bool autoSave = false)
        {
            var savedEntity = DbSet.Add(entity).Entity;

            if (autoSave)
            {
                DbContext.SaveChanges();
            }

            return savedEntity;
        }

        public override async Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var savedEntity = DbSet.Add(entity).Entity;

            if (autoSave)
            {
                await DbContext.SaveChangesAsync(GetCancellationToken(cancellationToken));
            }

            return savedEntity;
        }

        public override TEntity Update(TEntity entity, bool autoSave = false)
        {
            DbContext.Attach(entity);

            var updatedEntity = DbContext.Update(entity).Entity;

            if (autoSave)
            {
                DbContext.SaveChanges();
            }

            return updatedEntity;
        }

        public override async Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            DbContext.Attach(entity);

            var updatedEntity = DbContext.Update(entity).Entity;

            if (autoSave)
            {
                await DbContext.SaveChangesAsync(cancellationToken);
            }

            return updatedEntity;
        }

        public override void Delete(TEntity entity, bool autoSave = false)
        {
            DbSet.Remove(entity);

            if (autoSave)
            {
                DbContext.SaveChanges();
            }
        }

        public override async Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            DbSet.Remove(entity);

            if (autoSave)
            {
                await DbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public override List<TEntity> GetList(bool includeDetails = false)
        {
            return includeDetails
                ? WithDetails().ToList()
                : DbSet.ToList();
        }

        public override async Task<List<TEntity>> GetListAsync(bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return includeDetails
                ? await WithDetails().ToListAsync(cancellationToken)
                : await DbSet.ToListAsync(cancellationToken);
        }

        public  override long GetCount()
        {
            return DbSet.LongCount();
        }

        public override async Task<long> GetCountAsync(CancellationToken cancellationToken = default)
        {
            return await DbSet.LongCountAsync(cancellationToken);
        }

        protected override IQueryable<TEntity> GetQueryable()
        {
            return DbSet.AsQueryable();
        }

        public override void Delete(Expression<Func<TEntity, bool>> predicate, bool autoSave = false)
        {
            base.Delete(predicate, autoSave);

            if (autoSave)
            {
                DbContext.SaveChanges();
            }
        }

        public override async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, bool autoSave = false, CancellationToken cancellationToken = default)
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

        public virtual async Task EnsureCollectionLoadedAsync<TProperty>(
            TEntity entity,
            Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression,
            CancellationToken cancellationToken = default)
            where TProperty : class
        {
            await DbContext.Entry(entity).Collection(propertyExpression).LoadAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task EnsurePropertyLoadedAsync<TProperty>(
            TEntity entity,
            Expression<Func<TEntity, TProperty>> propertyExpression,
            CancellationToken cancellationToken = default)
            where TProperty : class
        {
            await DbContext.Entry(entity).Reference(propertyExpression).LoadAsync(GetCancellationToken(cancellationToken));
        }

        public override IQueryable<TEntity> WithDetails()
        {
            if (EntityOptions.DefaultWithDetailsFunc == null)
            {
                return base.WithDetails();
            }

            return EntityOptions.DefaultWithDetailsFunc(GetQueryable());
        }

        public override IQueryable<TEntity> WithDetails(params Expression<Func<TEntity, object>>[] propertySelectors)
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

       
    }
}