using ScrumPm.Domain.Common.PagedList;
using ScrumPm.Domain.Common.Persistence;

namespace ScrumPm.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    public abstract class EntityFrameworkCoreRepository<TContext, TEntity> : IRepository<TEntity>
        where TEntity : class
        where TContext : DbContext
    {
        private DbSet<TEntity> Set => Context.Set<TEntity>();

        private DbContext Context { get; }

        public IQueryable<TEntity> Queryable => Set.AsNoTracking();

        protected EntityFrameworkCoreRepository(IContextFactory<TContext> contextFactory)
        {
            Context = contextFactory.Create() as DbContext;
            Context.ChangeTracker.AutoDetectChangesEnabled = false;
            Context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public void Add(TEntity entity)
        {
            Set.Add(entity);
        }

        public async Task AddAsync(TEntity entity)
        {
            await Set.AddAsync(entity);
        }

        public void AddRange(params TEntity[] entities)
        {
            Set.AddRange(entities);
        }

        public async Task AddRangeAsync(params TEntity[] entities)
        {
            await Set.AddRangeAsync(entities);
        }

        public bool Any()
        {
            return Set.Any();
        }

        public bool Any(Expression<Func<TEntity, bool>> where)
        {
            return Set.Any(where);
        }

        public Task<bool> AnyAsync()
        {
            return Set.AnyAsync();
        }

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> where)
        {
            return Set.AnyAsync(where);
        }

        public long Count()
        {
            return Set.LongCount();
        }

        public long Count(Expression<Func<TEntity, bool>> where)
        {
            return Set.LongCount(where);
        }

        public Task<long> CountAsync()
        {
            return Set.LongCountAsync();
        }

        public Task<long> CountAsync(Expression<Func<TEntity, bool>> where)
        {
            return Set.LongCountAsync(where);
        }

        public void Delete(object key)
        {
            var entity = Set.Find(key);

            if (entity == null)
            {
                return;
            }

            Set.Remove(entity);
        }

        public TEntity FirstOrDefault(params Expression<Func<TEntity, object>>[] include)
        {
            return QueryableInclude(include).FirstOrDefault();
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> where,
            params Expression<Func<TEntity, object>>[] include)
        {
            return QueryableWhereInclude(where, include).FirstOrDefault();
        }

        public Task<TEntity> FirstOrDefaultAsync(params Expression<Func<TEntity, object>>[] include)
        {
            return QueryableInclude(include).FirstOrDefaultAsync();
        }

        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> where,
            params Expression<Func<TEntity, object>>[] include)
        {
            return QueryableWhereInclude(where, include).FirstOrDefaultAsync();
        }

        public TEntityResult FirstOrDefaultResult<TEntityResult>(Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, TEntityResult>> select)
        {
            return QueryableWhereSelect(where, select).FirstOrDefault();
        }

        public Task<TEntityResult> FirstOrDefaultResultAsync<TEntityResult>(Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, TEntityResult>> select)
        {
            return QueryableWhereSelect(where, select).FirstOrDefaultAsync();
        }

        public TEntity LastOrDefault(params Expression<Func<TEntity, object>>[] include)
        {
            return QueryableInclude(include).LastOrDefault();
        }

        public TEntity LastOrDefault(Expression<Func<TEntity, bool>> where,
            params Expression<Func<TEntity, object>>[] include)
        {
            return QueryableWhereInclude(where, include).LastOrDefault();
        }

        public Task<TEntity> LastOrDefaultAsync(params Expression<Func<TEntity, object>>[] include)
        {
            return QueryableInclude(include).LastOrDefaultAsync();
        }

        public Task<TEntity> LastOrDefaultAsync(Expression<Func<TEntity, bool>> where,
            params Expression<Func<TEntity, object>>[] include)
        {
            return QueryableWhereInclude(where, include).LastOrDefaultAsync();
        }

        public TEntityResult LastOrDefaultResult<TEntityResult>(Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, TEntityResult>> select)
        {
            return QueryableWhereSelect(where, select).LastOrDefault();
        }

        public Task<TEntityResult> LastOrDefaultResultAsync<TEntityResult>(Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, TEntityResult>> select)
        {
            return QueryableWhereSelect(where, select).LastOrDefaultAsync();
        }

        public IEnumerable<TEntity> List(params Expression<Func<TEntity, object>>[] include)
        {
            return QueryableInclude(include).ToList();
        }

        public IEnumerable<TEntity> List(Expression<Func<TEntity, bool>> where,
            params Expression<Func<TEntity, object>>[] include)
        {
            return QueryableWhereInclude(where, include).ToList();
        }

        public PagedList<TEntity> List(PagedListParameters parameters,
            params Expression<Func<TEntity, object>>[] include)
        {
            return new PagedList<TEntity>(QueryableInclude(include), parameters);
        }

        public PagedList<TEntity> List(PagedListParameters parameters, Expression<Func<TEntity, bool>> where,
            params Expression<Func<TEntity, object>>[] include)
        {
            return new PagedList<TEntity>(QueryableWhereInclude(where, include), parameters);
        }

        public async Task<IEnumerable<TEntity>> ListAsync(params Expression<Func<TEntity, object>>[] include)
        {
            return await QueryableInclude(include).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> ListAsync(Expression<Func<TEntity, bool>> where,
            params Expression<Func<TEntity, object>>[] include)
        {
            return await QueryableWhereInclude(where, include).ToListAsync();
        }

        public TEntity Select(long id)
        {
            return Set.Find(id);
        }

        public Task<TEntity> SelectAsync(long id)
        {
            return Set.FindAsync(id);
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> where,
            params Expression<Func<TEntity, object>>[] include)
        {
            return QueryableWhereInclude(where, include).SingleOrDefault();
        }

        public Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> where,
            params Expression<Func<TEntity, object>>[] include)
        {
            return QueryableWhereInclude(where, include).SingleOrDefaultAsync();
        }

        public TEntityResult SingleOrDefaultResult<TEntityResult>(Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, TEntityResult>> select)
        {
            return QueryableWhereSelect(where, select).SingleOrDefault();
        }

        public Task<TEntityResult> SingleOrDefaultResultAsync<TEntityResult>(Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, TEntityResult>> select)
        {
            return QueryableWhereSelect(where, select).SingleOrDefaultAsync();
        }

        public void Update(TEntity entity, object key)
        {
            var entityContext = Set.Find(key);

            if (entityContext == null)
            {
                return;
            }

            Context.Entry(entityContext).CurrentValues.SetValues(entity);
        }

        private IQueryable<TEntity> Include(IQueryable<TEntity> queryable, Expression<Func<TEntity, object>>[] include)
        {
            include?.ToList().ForEach(property => queryable = queryable.Include(property));
            return queryable;
        }

        private IQueryable<TEntity> QueryableInclude(Expression<Func<TEntity, object>>[] include)
        {
            return Include(Queryable, include);
        }

        private IQueryable<TEntity> QueryableWhereInclude(Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, object>>[] include)
        {
            return Include(Queryable.Where(where), include);
        }

        private IQueryable<TEntityResult> QueryableWhereSelect<TEntityResult>(Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, TEntityResult>> select)
        {
            return Queryable.Where(where).Select(select);
        }
    }
}