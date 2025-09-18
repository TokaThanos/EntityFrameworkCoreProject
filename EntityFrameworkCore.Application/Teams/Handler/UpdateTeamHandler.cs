using EntityFrameworkCore.Application.Interfaces;
using EntityFrameworkCore.Application.Teams.Commands;
using MediatR;

namespace EntityFrameworkCore.Application.Teams.Handler
{
    public class UpdateTeamHandler : TeamHandlerBase, IRequestHandler<UpdateTeamCommand>
    {
        public UpdateTeamHandler(ITeamService teamService) : base(teamService)
        {
        }

        public async Task Handle(UpdateTeamCommand request, CancellationToken cancellationToken)
        {
            await _teamService.UpdateTeamAsync(request.Id, request.TeamUpdateDto);
        }
    }
}
