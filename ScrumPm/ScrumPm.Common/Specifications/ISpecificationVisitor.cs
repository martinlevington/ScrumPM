

using System;

namespace ScrumPm.Domain.Common.Specifications
{
    public interface ISpecificationVisitor<T, TVisitor>  where TVisitor : ISpecificationVisitor<T, TVisitor>
    {
        void Visit(AndSpecification<T, TVisitor> spec);
        void Visit(OrSpecification<T, TVisitor> spec);
        void Visit(NotSpecification<T, TVisitor> spec);
    }
}
