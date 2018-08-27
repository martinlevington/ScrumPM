using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ScrumPm.Application.Products;
using ScrumPm.Common.Persistence;
using ScrumPm.Domain.Teams;
using ScrumPm.Domain.Tenants;
using ScrumPm.Persistence.Database;
using ScrumPm.Persistence.Teams.PersistenceModels;
using ProductOwner = ScrumPm.Domain.Teams.ProductOwner;
using Team = ScrumPm.Domain.Teams.Team;

namespace ScrumPm.Persistence.Teams.Repositories
{
    public class TeamRepository : Repository<Team, int, PersistenceModels.Team>, ITeamRepository
    {
        private readonly ScrumPMContext _context;

        public TeamRepository(ScrumPMContext context, IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _context = context;
        }

        public IEnumerable<Domain.Teams.Team> GetAllTeams(TenantId tenantId)
        {
           var teams =  _context.Teams.Select(x => x).ToList();

            var allTeams = new List<Team>();
            foreach (var dc in teams)
            {
                allTeams.Add(ConvertToDomain(tenantId, dc));
            }

            return allTeams;
        }

        public void Remove(Domain.Teams.Team team)
        {
            throw new NotImplementedException();
        }

        public void RemoveAll(IEnumerable<Domain.Teams.Team> teams)
        {
            throw new NotImplementedException();
        }

        public void Save(Domain.Teams.Team team)
        {
            throw new NotImplementedException();
        }

        public void SaveAll(IEnumerable<Domain.Teams.Team> teams)
        {
            throw new NotImplementedException();
        }

        public Team GetByName(TenantId tenantId, string name)
        {
            throw new NotImplementedException();
        }

        private Team ConvertToDomain(TenantId tenantId, PersistenceModels.Team persistenceModel)
        {

          return new Team(tenantId, persistenceModel.Name, ConvertToDomain(tenantId,persistenceModel.ProductOwner)) ;
        }


        private ProductOwner ConvertToDomain(TenantId tenantId, PersistenceModels.ProductOwner persistenceModel)
        {

            var productOwner = new ProductOwner(tenantId, persistenceModel.UserName,persistenceModel.FirstName,persistenceModel.LastName, persistenceModel.EmailAddress,persistenceModel.Modified);

            return productOwner;
        }
    }
}
