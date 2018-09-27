namespace ScrumPm.Domain.Common
{


    public interface IIdentity<T>
    {
        T Id { get; }
    }

}