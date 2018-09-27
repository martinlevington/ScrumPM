using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ScrumPm.Application.Teams;
using ScrumPm.Attributes;
using ScrumPm.Domain.Teams;
using ScrumPm.ViewModels.Teams;
using Serilog;

namespace ScrumPm.Controllers
{
    public class TeamController : Controller
    {
        private readonly ITeamApplicationService _teamApplicationService;
        private readonly ILogger<TeamController> _logger;


        public TeamController(ITeamApplicationService teamApplicationService, ILogger<TeamController> logger)
        {
            _teamApplicationService = teamApplicationService;
            _logger = logger;
        }

        [AutoMapFilter(SourceType = typeof(IEnumerable<Team>), DestinationType = typeof(IEnumerable<TeamViewModel>))]
        public IActionResult Index()
        {
           

            var Teams = _teamApplicationService.GetTeams();


            return View(Teams);
        }
    }
}