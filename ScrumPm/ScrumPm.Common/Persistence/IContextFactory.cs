namespace ScrumPm.Common.Persistence
{
    /// <summary>
    /// Context Factory Interface 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IContextFactory<T>
    {
        T GetContext();
    }
}
