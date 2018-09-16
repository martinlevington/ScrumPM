using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ScrumPm.Domain.Common.Persistence;
using ScrumPm.Domain.Common.Specifications;
using ScrumPm.Domain.Teams;
using ScrumPm.Domain.Teams.Specifications;
using ScrumPm.Domain.Tenants;
using ScrumPm.Persistence.Database;
using ScrumPm.Persistence.Teams.PersistenceModels;
using ScrumPm.Persistence.Teams.Specifications;
using ProductOwner = ScrumPm.Domain.Teams.ProductOwner;
using Team = ScrumPm.Domain.Teams.Team;

namespace ScrumPm.Persistence.Teams.Repositories
{
    public class TeamRepository : Repository<Team, int, TeamEf>, ITeamRepository
    {
        private readonly IMapper _mapper;


        public TeamRepository(IUnitOfWork<ScrumPMContext> unitOfWork, IMapper mapper) : base(unitOfWork)
        {
            _mapper = mapper;
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

      

        public IReadOnlyList<Team> Find(TenantId tenantId, ISpecification<Team, ITeamSpecificationVisitor> specification) 
        {
          
          //  var mappedSpecification = _mapper.Map<Expression<Func<TeamEf, bool>>>(specification);
            var visitor = new TeamEFExpressionVisitor();
            specification.Accept( visitor);
            var expression = visitor.Expr;

        
            var teams = UnitOfWork.GetContext().Teams.Include(t => t.ProductOwner).Where(expression).ToList();

            var allTeams = new List<Team>();
            foreach (var dc in teams)
            {
                allTeams.Add(ConvertToDomain(tenantId, dc));
            }

            return allTeams;
        }

        public Team GetById(TenantId tenantId, TeamId teamId)
        {
            var result = UnitOfWork.GetContext().Teams.Include(t => t.ProductOwner)
                .FirstOrDefault(x => x.Id == teamId.Id && x.TenantId == tenantId.Id);
            return ConvertToDomain(tenantId, result);
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

        /// <summary>
        /// Convert a Team Persistence model into a Team Domain Model
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="persistenceModel"></param>
        /// <returns></returns>
        private Team ConvertToDomain(TenantId tenantId, PersistenceModels.TeamEf persistenceModel)
        {

          return new Team(tenantId, new TeamId(persistenceModel.Id), persistenceModel.Name, ConvertToDomain(tenantId,persistenceModel.ProductOwner)) ;
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
