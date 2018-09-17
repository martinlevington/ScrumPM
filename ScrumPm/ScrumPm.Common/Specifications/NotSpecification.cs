namespace ScrumPm.Domain.Common.Specifications
{
    public class NotSpecification<T> : ISpecification<T> 
    {
        public ISpecification<T> Specification { get; set; }

        public NotSpecification(ISpecification<T> specification)
        {
            Specification = specification;
        }


        public bool IsSatisfiedBy(T item) => !Specification.IsSatisfiedBy(item);

        
    }
}
