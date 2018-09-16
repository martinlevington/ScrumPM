namespace ScrumPm.Persistence.Teams.Adapters
{
    public interface IPersistenceAdapter<in TTenant, TPersistence, TDomain>
    {
        TDomain ToDomain(TTenant tenantId,TPersistence persistenceModel);
        TPersistence ToPersistence(TTenant tenant, TDomain domain);

    }
}