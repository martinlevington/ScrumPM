using AutoMapper;
using ScrumPm.Domain.Teams;
using ScrumPm.Persistence.Models.Teams.PersistenceModels;
using ScrumPm.ViewModels.Teams;

namespace ScrumPm.Mapper
{
    public class TeamsProfile : Profile
    {

        public TeamsProfile()
        {
            CreateMap<Team, TeamViewModel>();

            CreateMap<TeamEf, Team>().ReverseMap();


        }
    }
}
