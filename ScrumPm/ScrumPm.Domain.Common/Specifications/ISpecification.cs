namespace ScrumPm.Domain.Common.Specifications
{

    public interface ISpecification <in T, in TVisitor>  where TVisitor : ISpecificationVisitor<T,TVisitor>
    {
        bool IsSatisfiedBy(T item);
        void Accept (TVisitor visitor);
    }

    
}