using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ScrumPm.Application.Teams;
using ScrumPm.Attributes;
using ScrumPm.Domain.Teams;
using ScrumPm.ViewModels.Teams;

namespace ScrumPm.Controllers
{
    public class TeamController : Controller
    {
        private readonly ITeamApplicationService _teamApplicationService;


        public TeamController(ITeamApplicationService teamApplicationService)
        {
            _teamApplicationService = teamApplicationService;
        }

        [AutoMapFilter(SourceType = typeof(IEnumerable<Team>), DestinationType = typeof(IEnumerable<TeamViewModel>))]
        public IActionResult Index()
        {

            var Teams = _teamApplicationService.GetTeams();


            return View(Teams);
        }
    }
}