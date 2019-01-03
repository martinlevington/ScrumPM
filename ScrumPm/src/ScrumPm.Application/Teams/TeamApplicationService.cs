using System;
using System.Collections.Generic;
using FluentResults;
using ScrumPm.Application.Teams.Commands;
using ScrumPm.Domain.Common.Specifications;
using ScrumPm.Domain.Common.Uow;
using ScrumPm.Domain.Teams;
using ScrumPm.Domain.Teams.Specifications;
using ScrumPm.Domain.Tenants;

namespace ScrumPm.Application.Teams
{
    public class TeamApplicationService : BaseApplicationService, ITeamApplicationService
    {
        private readonly ITeamMemberRepository _teamMemberRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IProductOwnerRepository _productOwnerRepository;

        public TeamApplicationService(ITeamMemberRepository teamMemberRepository, IProductOwnerRepository productOwnerRepository, ITeamRepository teamRepository, IUnitOfWorkManager<IUnitOfWork> untUnitOfWorkManager) : base(untUnitOfWorkManager)
        {
            _teamMemberRepository = teamMemberRepository;
            _productOwnerRepository = productOwnerRepository;
            _teamRepository = teamRepository;
        }

        public IEnumerable<Team> GetTeams(TenantId tenantId)
        {
           var teams =  _teamRepository.GetAllTeams(tenantId);
            return teams;
        }
     
        public Team GetTeam(TenantId id, TeamId teamId)
        {
            return _teamRepository.GetById(id, teamId);
        }

        public IEnumerable<Team> Search(TenantId tenantId, string search)
        {
           var searchSpecification = new TenantSpecification(tenantId).And(new TeamNameSearchSpecification(search));

           var teams =  _teamRepository.Find(tenantId, searchSpecification );
           return teams;
        }

        public Result EnableProductOwner(EnableProductOwnerCommand command)
        {
           

            try
            {
                ProductOwner productOwner; 
                var productOwnerResult = _productOwnerRepository.Get(command.TenantId, command.Username);
                if (productOwnerResult.IsSuccess)
                {
                    productOwner = productOwnerResult.Value;
                    productOwner.Enable(command.OccurredOn);
                }
                else
                {
                    productOwner = new ProductOwner(command.TenantId, command.Username, command.FirstName, command.LastName, command.EmailAddress, command.OccurredOn);
                   
                }

                _productOwnerRepository.Save(command.TenantId, productOwner);
        
            }
            catch (Exception ex)
            {
                CurrentUnitOfWork.Rollback();
            }

            return Results.Ok();
        }

    }
}
