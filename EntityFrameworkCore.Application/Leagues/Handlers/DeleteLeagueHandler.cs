using EntityFrameworkCore.Application.Interfaces;
using EntityFrameworkCore.Application.Leagues.Commands;
using MediatR;

namespace EntityFrameworkCore.Application.Leagues.Handlers
{
    public class DeleteLeagueHandler : LeagueHandlerBase, IRequestHandler<DeleteLeagueCommand>
    {
        public DeleteLeagueHandler(ILeagueService leagueService) : base(leagueService) { }

        public async Task Handle(DeleteLeagueCommand request, CancellationToken cancellationToken)
        {
            await _leagueService.DeleteLeagueByIdAsync(request.Id);
        }
    }
}
