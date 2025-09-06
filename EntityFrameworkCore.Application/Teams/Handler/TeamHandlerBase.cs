using EntityFrameworkCore.Application.Interfaces;

namespace EntityFrameworkCore.Application.Teams.Handler
{
    public abstract class TeamHandlerBase
    {
        protected readonly ITeamService _teamService;

        protected TeamHandlerBase(ITeamService teamService)
        {
            _teamService = teamService;
        }
    }
}
