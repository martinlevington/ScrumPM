using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ScrumPm.Application.Teams;
using ScrumPm.Attributes;
using ScrumPm.BindingModels;
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
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ILogger<TeamController> _logger;

        private TenantId _tenantId = new TenantId(new Guid("544060C5-4F5F-4EA6-AC8E-7100D7E87CCB"));


        public TeamController(ITeamApplicationService teamApplicationService, IUnitOfWorkManager unitOfWorkManager, ILogger<TeamController> logger)
        {
            _teamApplicationService = teamApplicationService;
            _unitOfWorkManager = unitOfWorkManager;
            _logger = logger;
        }

        [AutoMapFilter(SourceType = typeof(IEnumerable<Team>), DestinationType = typeof(IEnumerable<TeamViewModel>))]

        [Route("")]      // Combines to define the route template "Home"
        [Route("Index")] // Combines to define the route template "Home/Index"
        [Route("/")]     // Doesn't combine, defines the route template ""

        public IActionResult Index()
        {

            using (var unitOfWork = _unitOfWorkManager.Create())
            {
                _logger.LogDebug("TeamController: Index");

                var teams = _teamApplicationService.GetTeams(_tenantId);

                _logger.LogInformation("End TeamController: Index");
                return View(teams);
            }
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

        public IActionResult Search(SearchBindingModel searchTerm)
        {

            var teams = _teamApplicationService.Search(_tenantId, searchTerm.Search);

            return View("Index", teams);
        }
    }
}
