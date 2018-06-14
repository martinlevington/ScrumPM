namespace ScrumPm.Application.Products
{
    using System.Collections.Generic;
    using ScrumPm.Domain.Teams;
    using ScrumPm.Domain.Tenants;

    public interface ITeamRepository
    {
        ICollection<Team> GetAllTeams(TenantId tenantId);

        void Remove(Team team);

        void RemoveAll(IEnumerable<Team> teams);

        void Save(Team team);

        void SaveAll(IEnumerable<Team> teams);

        Team GetByName(TenantId tenantId, string name);
    }
}