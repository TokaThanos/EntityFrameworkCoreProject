using EntityFrameworkCore.Application.Dtos;
using EntityFrameworkCore.Application.Interfaces;
using EntityFrameworkCore.Application.Leagues.Queries;
using MediatR;

namespace EntityFrameworkCore.Application.Leagues.Handlers
{
    public class GetLeagueByIdHandler : LeagueHandlerBase, IRequestHandler<GetLeagueByIdQuery, LeagueReadInfoDto?>
    {
        public GetLeagueByIdHandler(ILeagueService leagueService) : base(leagueService) { }

        public async Task<LeagueReadInfoDto?> Handle(GetLeagueByIdQuery request, CancellationToken cancellationToken)
        {
            return await _leagueService.GetLeagueByIdAsync(request.Id);
        }
    }
}
