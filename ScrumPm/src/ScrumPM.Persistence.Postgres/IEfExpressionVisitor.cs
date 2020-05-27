using System;
using System.Linq.Expressions;
using ScrumPm.Domain.Common.Specifications;

namespace ScrumPM.Persistence.Postgres
{
    public interface IEfExpressionVisitor<TEntity, TVisitor, TItem> where TVisitor : ISpecificationVisitor<TItem, TVisitor>
    {
        Expression<Func<TEntity, bool>> Expr { get; }
        Expression<Func<TEntity, bool>> ConvertSpecToExpression (ISpecification<TItem,TVisitor> spec);
        void Visit (AndSpecification<TItem, TVisitor> spec);
        void Visit(OrSpecification<TItem, TVisitor> spec);
        void Visit(NotSpecification<TItem, TVisitor> spec);
    }
}