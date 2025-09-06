using EntityFrameworkCore.Application.Dtos;
using EntityFrameworkCore.Application.Interfaces;
using EntityFrameworkCore.Application.Teams.Queries;
using MediatR;

namespace EntityFrameworkCore.Application.Teams.Handler
{
    public class GetTeamsHandler : TeamHandlerBase, IRequestHandler<GetTeamsQuery, IEnumerable<TeamReadDto>>
    {
        public GetTeamsHandler(ITeamService teamService) : base(teamService)
        {
        }

        public async Task<IEnumerable<TeamReadDto>> Handle(GetTeamsQuery request, CancellationToken cancellationToken)
        {
            return await _teamService.GetAllTeamsAsync();
        }
    }
}
