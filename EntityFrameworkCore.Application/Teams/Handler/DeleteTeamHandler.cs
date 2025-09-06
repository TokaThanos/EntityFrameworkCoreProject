using EntityFrameworkCore.Application.Interfaces;
using EntityFrameworkCore.Application.Teams.Commands;
using MediatR;

namespace EntityFrameworkCore.Application.Teams.Handler
{
    public class DeleteTeamHandler : TeamHandlerBase, IRequestHandler<DeleteTeamCommand>
    {
        public DeleteTeamHandler(ITeamService teamService) : base(teamService)
        {
        }

        public async Task Handle(DeleteTeamCommand request, CancellationToken cancellationToken)
        {
            await _teamService.DeleteTeamByIdAsync(request.Id);
        }
    }
}
