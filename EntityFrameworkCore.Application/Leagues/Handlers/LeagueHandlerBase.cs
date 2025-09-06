using EntityFrameworkCore.Application.Interfaces;

namespace EntityFrameworkCore.Application.Leagues.Handlers
{
    public abstract class LeagueHandlerBase
    {
        protected readonly ILeagueService _leagueService;

        protected LeagueHandlerBase(ILeagueService leagueService)
        {
            _leagueService = leagueService;
        }
    }
}
