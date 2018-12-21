using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ScrumPm.Domain.Common.DependancyInjection;
using ScrumPm.Domain.Common.Guids;
using ScrumPm.Domain.Common.MultiTenant;
using ScrumPm.Persistence.Expressions;

namespace ScrumPm.Persistence.EntityFrameworkCore
{
    public abstract class EfCoreContext<TDbContext> : DbContext, IEfCoreDbContext, ITransientDependency
        where TDbContext : DbContext
    {

        public ICurrentTenant CurrentTenant { get; set; }

        protected virtual Guid? CurrentTenantId => CurrentTenant?.Id;

        protected EfCoreContext(DbContextOptions<TDbContext> options)
            : base(options)
        {
     
     
        }

        protected virtual Expression<Func<TEntity, bool>> CreateFilterExpression<TEntity>()
            where TEntity : class
        {
            Expression<Func<TEntity, bool>> expression = null;

      

            //if (typeof(IMultiTenant).IsAssignableFrom(typeof(TEntity)))
            //{
            //    /* This condition should normally be defined as below:
            //     * !IsMayHaveTenantFilterEnabled || ((IMayHaveTenant)e).TenantId == CurrentTenantId
            //     * But this causes a problem with EF Core (see https://github.com/aspnet/EntityFrameworkCore/issues/9502)
            //     * So, we made a workaround to make it working. It works same as above.
            //     */
            //    Expression<Func<TEntity, bool>> multiTenantFilter = e => ((IMultiTenant)e).TenantId == CurrentTenantId || (((IMultiTenant)e).TenantId == CurrentTenantId) == IsMultiTenantFilterEnabled;
            //    expression = expression == null ? multiTenantFilter : CombineExpressions(expression, multiTenantFilter);
            //}

            return expression;
        }

        protected virtual Expression<Func<T, bool>> CombineExpressions<T>(Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(expression1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expression1.Body);

            var rightVisitor = new ReplaceExpressionVisitor(expression2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expression2.Body);

            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(left, right), parameter);
        }

    }
}
