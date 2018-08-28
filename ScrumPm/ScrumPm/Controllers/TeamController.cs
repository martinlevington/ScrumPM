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
            _logger.LogInformation("TeamController: Index");
            _logger.LogDebug("TeamController: Index");
            _logger.LogError("TeamController: Index");
           
            Log.Error("Team Error");

            _logger.LogInformation("Before");

            using (_logger.BeginScope("Some name"))
            using (_logger.BeginScope(42))
            using (_logger.BeginScope("Formatted {WithValue}", 12345))
            using (_logger.BeginScope(new Dictionary<string, object> { ["ViaDictionary"] = 100 }))
            {
                _logger.LogInformation("Hello from the Index!");
                _logger.LogDebug("Hello is done");
            }

            _logger.LogInformation("After");

            var Teams = _teamApplicationService.GetTeams();


            return View(Teams);
        }
    }
}