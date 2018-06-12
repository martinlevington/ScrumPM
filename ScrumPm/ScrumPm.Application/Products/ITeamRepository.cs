using System.Collections.Generic;

namespace ScrumPm.Application.Products
{
    public interface ITeamRepository
    {
        ICollection<Team> GetAllTeams(Tenants.TenantId tenantId);

        void Remove(Team team);

        void RemoveAll(IEnumerable<Team> teams);

        void Save(Team team);

        void SaveAll(IEnumerable<Team> teams);

        Team GetByName(Tenants.TenantId tenantId, string name);
    }
}