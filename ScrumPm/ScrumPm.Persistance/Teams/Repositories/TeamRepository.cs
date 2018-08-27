using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ScrumPm.Application.Products;
using ScrumPm.Common.Persistence;
using ScrumPm.Domain.Tenants;
using ScrumPm.Persistence.Database;
using ProductOwner = ScrumPm.Domain.Teams.ProductOwner;
using Team = ScrumPm.Domain.Teams.Team;

namespace ScrumPm.Persistence.Teams.Repositories
{
    public class TeamRepository : Repository<Team, int, PersistenceModels.Team>, ITeamRepository
    {


        public TeamRepository(IUnitOfWork<ScrumPMContext> unitOfWork) : base(unitOfWork)
        {
      
        }

        public IEnumerable<Team> GetAllTeams(TenantId tenantId)
        {
           var teams =  UnitOfWork.GetContext().Teams.Include(t => t.ProductOwner).Select(x => x).ToList();

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

        /// <summary>
        /// Convert a Team Persistence model into a Team Domain Model
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="persistenceModel"></param>
        /// <returns></returns>
        private Team ConvertToDomain(TenantId tenantId, PersistenceModels.Team persistenceModel)
        {

          return new Team(tenantId, persistenceModel.Name, ConvertToDomain(tenantId,persistenceModel.ProductOwner)) ;
        }

        /// <summary>
        /// Convert a Product Owner Persistence model into a 'ProductOwner' Domain Model
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="persistenceModel"></param>
        /// <returns></returns>
        private ProductOwner ConvertToDomain(TenantId tenantId, PersistenceModels.ProductOwner persistenceModel)
        {

            var productOwner = new ProductOwner(tenantId, persistenceModel.UserName,persistenceModel.FirstName,persistenceModel.LastName, persistenceModel.EmailAddress,persistenceModel.Modified);

            return productOwner;
        }
    }
}
