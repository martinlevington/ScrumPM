using System;
using System.Collections.Generic;
using System.Linq;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ScrumPm.Domain.Common.Specifications;
using ScrumPm.Domain.Teams;
using ScrumPm.Domain.Teams.Specifications;
using ScrumPm.Domain.Tenants;
using ScrumPm.Persistence.Models.Teams;
using ScrumPm.Persistence.Models.Teams.PersistenceModels;
using ScrumPM.Persistence.Postgres.Teams.Specifications;

namespace ScrumPM.Persistence.Postgres.Teams.Repositories
{
    public class TeamRepository : EfCoreRepository<ScrumPmContext, TeamEf,Guid > , ITeamRepository
    {
      
        private readonly ITeamAdapterFactory _teamAdapterFactory;


        public TeamRepository(IDbContextProvider<ScrumPmContext> dbContextProvider,
            ITeamAdapterFactory teamAdapterFactory) : base(dbContextProvider)
        {
          
            _teamAdapterFactory = teamAdapterFactory;
        }

        public IEnumerable<Team> GetAllTeams(TenantId tenantId)
        {
            var teams = DbContext.Teams.Include(t => t.ProductOwnerEf).Select(x => x).ToList();

            var allTeams = new List<Team>();
            foreach (var team in teams)
            {
                allTeams.Add(_teamAdapterFactory.Create(tenantId,team));
            }

            return allTeams;
        }


        public Result<IReadOnlyList<Team>> Find(TenantId tenantId,
            ISpecification<Team, ITeamSpecificationVisitor> specification)
        {
            var visitor = new TeamEfExpressionVisitor();
            specification.Accept(visitor);
            var expression = visitor.Expr;

            var teams = DbContext.Teams.Include(t => t.ProductOwnerEf).Where(expression).ToList();

            var allTeams = new List<Team>();
            foreach (var team in teams)
            {
                allTeams.Add(_teamAdapterFactory.Create(tenantId,team));
            }

            if (allTeams.Any())
            {
                return Results.Ok<IReadOnlyList<Team>>(allTeams);
            }

            return Results.Fail<IReadOnlyList<Team>>(errorMessage: "Unable To find " + nameof(Team) + " Object");
        }

        public Team GetById(TenantId tenantId, TeamId teamId)
        {
            var team = DbContext.Teams.Include(t => t.ProductOwnerEf)
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

        public Result<Team> Save(TenantId tenantId, Team team)
        {
            var teamEf = new TeamEf()
            {
                Id = team.Id.GetId(),
                Name = team.Name,
                ProductOwnerId = team.ProductOwner.ProductOwnerId,
                TenantId = team.TenantId.Id
            };

            DbContext.Teams.Add(teamEf);

            return Results.Ok<Team>(_teamAdapterFactory.Create(tenantId,teamEf));
        }

        public Result<Team> Update(TenantId tenantId, Team team)
        {
            var teamEf = new TeamEf()
            {
                Id = team.Id.GetId(),
                Name = team.Name,
                ProductOwnerId = team.ProductOwner.ProductOwnerId,
                TenantId = team.TenantId.Id
            };

            DbContext.Teams.Update(teamEf);

            return Results.Ok<Team>(_teamAdapterFactory.Create(tenantId,teamEf));
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