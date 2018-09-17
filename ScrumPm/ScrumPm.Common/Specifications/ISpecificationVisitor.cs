



namespace ScrumPm.Domain.Common.Specifications
{
    public interface ISpecificationVisitor<T, TVisitor> where TVisitor : ISpecificationVisitor<T, TVisitor>
    {
        void Visit(AndSpecification<T> spec);
        void Visit(OrSpecification<T> spec);
        void Visit(NotSpecification<T> spec);
    }
}
