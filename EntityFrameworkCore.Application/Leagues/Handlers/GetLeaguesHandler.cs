using EntityFrameworkCore.Application.Dtos;
using EntityFrameworkCore.Application.Interfaces;
using EntityFrameworkCore.Application.Leagues.Queries;
using MediatR;

namespace EntityFrameworkCore.Application.Leagues.Handlers
{
    public class GetLeaguesHandler : LeagueHandlerBase, IRequestHandler<GetLeaguesQuery, IEnumerable<LeagueReadDto>>
    {
        public GetLeaguesHandler(ILeagueService leagueService) : base(leagueService) { }

        public async Task<IEnumerable<LeagueReadDto>> Handle(GetLeaguesQuery request, CancellationToken cancellationToken)
        {
            return await _leagueService.GetAllLeaguesAsync();
        }
    }
}
