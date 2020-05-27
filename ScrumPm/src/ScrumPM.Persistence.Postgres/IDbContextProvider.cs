namespace ScrumPM.Persistence.Postgres
{
    public interface IDbContextProvider<out TDbContext>
        where TDbContext : IEfCoreDbContext
    {
        TDbContext GetDbContext();
    }
}
