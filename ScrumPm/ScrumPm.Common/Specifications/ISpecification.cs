namespace ScrumPm.Domain.Common.Specifications
{

    public interface ISpecification <in T>  
    {
        bool IsSatisfiedBy(T item);
 
    }

    
}