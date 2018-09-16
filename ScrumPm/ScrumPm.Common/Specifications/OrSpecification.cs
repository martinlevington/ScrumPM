namespace ScrumPm.Domain.Common.Specifications
{
    public class OrSpecification<T, TVisitor> : ISpecification<T, TVisitor> 
        where TVisitor : ISpecificationVisitor<T, TVisitor>
    {
        private readonly ISpecification<T, TVisitor> _left;
        private readonly ISpecification<T, TVisitor> _right;


        public OrSpecification(ISpecification<T, TVisitor> left, ISpecification<T,TVisitor> right) {
            _right = right;
            _left = left;
        }

        public ISpecification<T, TVisitor> Right => _right;
        public ISpecification<T, TVisitor> Left => _left;

        public bool IsSatisfiedBy(T entity)
        {
            return _right.IsSatisfiedBy(entity) || _left.IsSatisfiedBy(entity);
        }

        public void Accept(TVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
