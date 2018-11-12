using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ScrumPm.Domain.Common.Persistence;
using ScrumPm.Domain.Common.Specifications;
using ScrumPm.Domain.Teams;
using ScrumPm.Domain.Teams.Specifications;
using ScrumPm.Domain.Tenants;
using ScrumPm.Persistence.Database;
using ScrumPm.Persistence.Teams.PersistenceModels;
using ScrumPm.Persistence.Teams.Specifications;

namespace ScrumPm.Persistence.Teams.Repositories
{
    public class TeamRepository : Repository<Team, int, TeamEf>, ITeamRepository
    {
      
        private readonly ITeamAdapterFactory _teamAdapterFactory;


        public TeamRepository(IUnitOfWork<ScrumPmContext> unitOfWork,
            ITeamAdapterFactory teamAdapterFactory) : base(unitOfWork)
        {
          
            _teamAdapterFactory = teamAdapterFactory;
        }

        public IEnumerable<Team> GetAllTeams(TenantId tenantId)
        {
            var teams = UnitOfWork.GetContext().Teams.Include(t => t.ProductOwner).Select(x => x).ToList();

            var allTeams = new List<Team>();
            foreach (var team in teams)
            {
                allTeams.Add(_teamAdapterFactory.Create(tenantId,team));
            }

            return allTeams;
        }


        public IReadOnlyList<Team> Find(TenantId tenantId,
            ISpecification<Team, ITeamSpecificationVisitor> specification)
        {
            var visitor = new TeamEfExpressionVisitor();
            specification.Accept(visitor);
            var expression = visitor.Expr;


            var teams = UnitOfWork.GetContext().Teams.Include(t => t.ProductOwner).Where(expression).ToList();

            var allTeams = new List<Team>();
            foreach (var team in teams)
            {
                allTeams.Add(_teamAdapterFactory.Create(tenantId,team));
            }

            return allTeams;
        }

        public Team GetById(TenantId tenantId, TeamId teamId)
        {
            var team = UnitOfWork.GetContext().Teams.Include(t => t.ProductOwner)
                .FirstOrDefault(x => x.Id == teamId.Id && x.TenantId == tenantId.Id);
            return _teamAdapterFactory.Create(tenantId, team);
        }

        public void Remove(Team team)
        {
            throw new NotImplementedException();
        }

        public void RemoveAll(IEnumerable<Team> teams)
        {
            throw new NotImplementedException();
        }

        public void Save(Team team)
        {
            throw new NotImplementedException();
        }

        public void SaveAll(IEnumerable<Team> teams)
        {
            throw new NotImplementedException();
        }

        public Team GetByName(TenantId tenantId, string name)
        {
            throw new NotImplementedException();
        }

       
    }
}