using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ScrumPm.Domain.Common.PagedList;

namespace ScrumPm.Domain.Common.Persistence
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Queryable { get; }

        void Add(TEntity entity);

        Task AddAsync(TEntity entity);

        void AddRange(params TEntity[] entities);

        Task AddRangeAsync(params TEntity[] entities);

        bool Any();

        bool Any(Expression<Func<TEntity, bool>> where);

        Task<bool> AnyAsync();

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> where);

        long Count();

        long Count(Expression<Func<TEntity, bool>> where);

        Task<long> CountAsync();

        Task<long> CountAsync(Expression<Func<TEntity, bool>> where);

        void Delete(object key);

        TEntity FirstOrDefault(params Expression<Func<TEntity, object>>[] include);

        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> where,
            params Expression<Func<TEntity, object>>[] include);

        Task<TEntity> FirstOrDefaultAsync(params Expression<Func<TEntity, object>>[] include);

        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> where,
            params Expression<Func<TEntity, object>>[] include);

        TEntityResult FirstOrDefaultResult<TEntityResult>(Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, TEntityResult>> select);

        Task<TEntityResult> FirstOrDefaultResultAsync<TEntityResult>(Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, TEntityResult>> select);

        TEntity LastOrDefault(params Expression<Func<TEntity, object>>[] include);

        TEntity LastOrDefault(Expression<Func<TEntity, bool>> where,
            params Expression<Func<TEntity, object>>[] include);

        Task<TEntity> LastOrDefaultAsync(params Expression<Func<TEntity, object>>[] include);

        Task<TEntity> LastOrDefaultAsync(Expression<Func<TEntity, bool>> where,
            params Expression<Func<TEntity, object>>[] include);

        TEntityResult LastOrDefaultResult<TEntityResult>(Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, TEntityResult>> select);

        Task<TEntityResult> LastOrDefaultResultAsync<TEntityResult>(Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, TEntityResult>> select);

        IEnumerable<TEntity> List(params Expression<Func<TEntity, object>>[] include);

        IEnumerable<TEntity> List(Expression<Func<TEntity, bool>> where,
            params Expression<Func<TEntity, object>>[] include);

        PagedList<TEntity> List(PagedListParameters parameters, params Expression<Func<TEntity, object>>[] include);

        PagedList<TEntity> List(PagedListParameters parameters, Expression<Func<TEntity, bool>> where,
            params Expression<Func<TEntity, object>>[] include);

        Task<IEnumerable<TEntity>> ListAsync(params Expression<Func<TEntity, object>>[] include);

        Task<IEnumerable<TEntity>> ListAsync(Expression<Func<TEntity, bool>> where,
            params Expression<Func<TEntity, object>>[] include);

        TEntity Select(long id);

        Task<TEntity> SelectAsync(long id);

        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> where,
            params Expression<Func<TEntity, object>>[] include);

        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> where,
            params Expression<Func<TEntity, object>>[] include);

        TEntityResult SingleOrDefaultResult<TEntityResult>(Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, TEntityResult>> select);

        Task<TEntityResult> SingleOrDefaultResultAsync<TEntityResult>(Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, TEntityResult>> select);

        void Update(TEntity entity, object key);
    }
}