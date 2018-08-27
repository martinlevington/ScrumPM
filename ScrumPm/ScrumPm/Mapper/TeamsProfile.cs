using AutoMapper;
using ScrumPm.Domain.Teams;
using ScrumPm.ViewModels.Teams;

namespace ScrumPm.Mapper
{
    public class TeamsProfile : Profile
    {

        public TeamsProfile()
        {
            CreateMap<Team, TeamViewModel>();
        }
    }
}
