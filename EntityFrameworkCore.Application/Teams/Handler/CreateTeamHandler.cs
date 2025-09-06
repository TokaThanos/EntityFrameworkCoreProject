using EntityFrameworkCore.Application.Dtos;
using EntityFrameworkCore.Application.Interfaces;
using EntityFrameworkCore.Application.Teams.Commands;
using MediatR;

namespace EntityFrameworkCore.Application.Teams.Handler
{
    public class CreateTeamHandler : TeamHandlerBase, IRequestHandler<CreateTeamCommand, TeamReadDto>
    {
        public CreateTeamHandler(ITeamService teamService) : base(teamService)
        {
        }

        public async Task<TeamReadDto> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
        {
            return await _teamService.AddTeamAsync(request.TeamCreateDto);
        }
    }
}
