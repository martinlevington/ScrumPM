namespace ScrumPm.Domain.Common.Specifications
{
    public class OrSpecification<T> : ISpecification<T> 
        
    {
        private readonly ISpecification<T> _left;
        private readonly ISpecification<T> _right;


        public OrSpecification(ISpecification<T> left, ISpecification<T> right) {
            _right = right;
            _left = left;
        }

        public ISpecification<T> Right => _right;
        public ISpecification<T> Left => _left;

        public bool IsSatisfiedBy(T entity)
        {
            return _right.IsSatisfiedBy(entity) || _left.IsSatisfiedBy(entity);
        }

       
    }
}
