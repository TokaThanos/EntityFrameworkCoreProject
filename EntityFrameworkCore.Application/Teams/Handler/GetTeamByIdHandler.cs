using EntityFrameworkCore.Application.Dtos;
using EntityFrameworkCore.Application.Interfaces;
using EntityFrameworkCore.Application.Teams.Queries;
using MediatR;

namespace EntityFrameworkCore.Application.Teams.Handler
{
    public class GetTeamByIdHandler : TeamHandlerBase, IRequestHandler<GetTeamByIdQuery, TeamReadInfoDto?>
    {
        public GetTeamByIdHandler(ITeamService teamService) : base(teamService)
        {
        }

        public async Task<TeamReadInfoDto?> Handle(GetTeamByIdQuery request, CancellationToken cancellationToken)
        {
            return await _teamService.GetTeamByIdAsync(request.Id);
        }
    }
}
