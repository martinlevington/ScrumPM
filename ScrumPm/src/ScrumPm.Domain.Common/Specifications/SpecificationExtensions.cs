namespace ScrumPm.Domain.Common.Specifications
{
    public static class SpecificationExtensions
    {
        public static ISpecification<T, TVisitor> And<T, TVisitor>(this ISpecification<T, TVisitor> left, ISpecification<T, TVisitor> right) 
            where TVisitor : ISpecificationVisitor<T, TVisitor>
        {
            return new AndSpecification<T, TVisitor> (left, right);
        }

        public static ISpecification<T, TVisitor> Or<T, TVisitor>(this ISpecification<T, TVisitor> left, ISpecification<T, TVisitor> right) 
            where TVisitor : ISpecificationVisitor<T, TVisitor>
        {
            return new OrSpecification<T, TVisitor> (left, right);
        }

        public static ISpecification<T, TVisitor> Not<T, TVisitor>(this ISpecification<T, TVisitor> specification) 
            where TVisitor : ISpecificationVisitor<T, TVisitor>
        {
            return new NotSpecification<T, TVisitor> (specification);
        }
    }
}
