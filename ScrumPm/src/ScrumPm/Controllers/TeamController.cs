using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ScrumPm.Application.Teams;
using ScrumPm.Application.Teams.Commands;
using ScrumPm.AspNetCore.Common.Attributes;
using ScrumPm.BindingModels;
using ScrumPm.Domain.Common.DateTimes;
using ScrumPm.Domain.Common.Uow;
using ScrumPm.Domain.Teams;
using ScrumPm.Domain.Tenants;
using ScrumPm.ViewModels.Teams;

namespace ScrumPm.Controllers
{
    [Route("[controller]/[action]")]
    public class TeamController : Controller
    {
        private readonly ITeamApplicationService _teamApplicationService;
        private readonly IUnitOfWorkFactory<IUnitOfWork> _unitOfWorkFactory;
        private readonly ILogger<TeamController> _logger;
        private readonly IDateTimeClock _dateTimeClock;

        private TenantId _tenantId = new TenantId(new Guid("544060C5-4F5F-4EA6-AC8E-7100D7E87CCB"));


        public TeamController(ITeamApplicationService teamApplicationService, IUnitOfWorkFactory<IUnitOfWork> unitOfWorkFactory, ILogger<TeamController> logger, IDateTimeClock dateTimeClock)
        {
            _teamApplicationService = teamApplicationService;
            _unitOfWorkFactory = unitOfWorkFactory;
            _logger = logger;
            _dateTimeClock = dateTimeClock;
        }

        [AutoMapFilter(SourceType = typeof(IEnumerable<Team>), DestinationType = typeof(IEnumerable<TeamViewModel>))]

        [Route("")]      // Combines to define the route template "Home"
        [Route("Index")] // Combines to define the route template "Home/Index"
        [Route("/")]     // Doesn't combine, defines the route template ""

        public IActionResult Index()
        {

            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                _logger.LogDebug("TeamController: Index");

                var teams = _teamApplicationService.GetTeams(_tenantId);

                _logger.LogInformation("End TeamController: Index");
                unitOfWork.Complete();

                return View(teams);
            }
        }

        [UnitOfWork]
        public IActionResult EnableProductOwner()
        {
            var cmd = new EnableProductOwnerCommand(
                _tenantId,"owner1","fred","clause","me@email.com",_dateTimeClock.Now, new Guid("8730F86E-FD47-49EE-9356-29B4D941EA88")
            );

            _teamApplicationService.EnableProductOwner(cmd);

            return View();
        }

        public IActionResult EditView(Guid teamId)
        {
            var team = _teamApplicationService.GetTeam(_tenantId, new TeamId(teamId));

            return View(team);
        }

        [HttpGet("{teamId}")]
        [AutoMapFilter(SourceType = typeof(Team), DestinationType = typeof(TeamViewModel))]
        public ActionResult Details(string teamId)
        {
            if (string.IsNullOrWhiteSpace(teamId)) throw new ArgumentNullException(nameof(teamId));

            var team = _teamApplicationService.GetTeam(_tenantId, new TeamId(teamId));

            return View(team);
        }

        [HttpPost]
        [AutoMapFilter(SourceType = typeof(IEnumerable<Team>), DestinationType = typeof(IEnumerable<TeamViewModel>))]
        [UnitOfWork]
        public IActionResult Search(SearchBindingModel searchTerm)
        {
           
                var teams = _teamApplicationService.Search(_tenantId, searchTerm.Search);

                return View("Index", teams);
            
        }
    }
}
