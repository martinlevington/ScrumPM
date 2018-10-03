namespace ScrumPm.Domain.Common.Specifications
{
    public class NotSpecification<T, TVisitor> : ISpecification<T, TVisitor> where TVisitor : ISpecificationVisitor<T, TVisitor>
    {
        public ISpecification<T, TVisitor> Specification { get; set; }

        public NotSpecification(ISpecification<T,TVisitor> specification)
        {
            Specification = specification;
        }


        public bool IsSatisfiedBy(T item) => !Specification.IsSatisfiedBy(item);

        public void Accept(TVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
