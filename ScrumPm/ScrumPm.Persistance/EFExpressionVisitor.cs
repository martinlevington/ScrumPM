using System;
using System.Linq;
using System.Linq.Expressions;
using ScrumPm.Domain.Common.Specifications;


namespace ScrumPm.Persistence
{
    public abstract class EFExpressionVisitor<TEntity, TVisitor, TItem>
        where TVisitor : ISpecificationVisitor<TItem, TVisitor >
    {
        public Expression<Func<TEntity, bool>> Expr { get; protected set; }

        public abstract Expression<Func<TEntity, bool>> ConvertSpecToExpression (ISpecification<TItem,TVisitor> spec);

        public void Visit (AndSpecification<TItem, TVisitor> spec)
        {
            var leftExpr = ConvertSpecToExpression (spec.Left);
            var rightExpr = ConvertSpecToExpression (spec.Right);

            var visitor = new SwapVisitor(leftExpr.Parameters[0], rightExpr.Parameters[0]);

            var exprBody = Expression.AndAlso (visitor.Visit(leftExpr.Body), rightExpr.Body);
            Expr = Expression.Lambda<Func<TEntity, bool>> (exprBody, rightExpr.Parameters);
        }

        public void Visit(OrSpecification<TItem, TVisitor> spec)
        {
            var leftExpr = ConvertSpecToExpression (spec.Left);
            var rightExpr = ConvertSpecToExpression (spec.Right);

            var visitor = new SwapVisitor(leftExpr.Parameters[0], rightExpr.Parameters[0]);

            var exprBody = Expression.OrElse (visitor.Visit(leftExpr.Body), rightExpr.Body);
            Expr = Expression.Lambda<Func<TEntity, bool>> (exprBody, rightExpr.Parameters);

        }

        public void Visit(NotSpecification<TItem, TVisitor> spec)
        {
            var specExpr = ConvertSpecToExpression (spec.Specification);


            var exprBody = Expression.Not (specExpr.Body);
            Expr = Expression.Lambda<Func<TEntity, bool>> (exprBody, specExpr.Parameters.Single ());

        }

    }

    public class SwapVisitor : ExpressionVisitor
    {
        private readonly Expression from, to;

        public SwapVisitor(Expression from, Expression to)
        {
            this.from = from;
            this.to = to;
        }

        public override Expression Visit(Expression node)
        {
            return node == from ? to : base.Visit(node);
        }
    }
}
