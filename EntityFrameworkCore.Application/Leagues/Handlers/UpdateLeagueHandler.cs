using EntityFrameworkCore.Application.Interfaces;
using EntityFrameworkCore.Application.Leagues.Commands;
using MediatR;

namespace EntityFrameworkCore.Application.Leagues.Handlers
{
    public class UpdateLeagueHandler : LeagueHandlerBase, IRequestHandler<UpdateLeagueCommand>
    {
        public UpdateLeagueHandler(ILeagueService leagueService) : base(leagueService) { }

        public async Task Handle(UpdateLeagueCommand request, CancellationToken cancellationToken)
        {
            await _leagueService.UpdateLeagueAsync(request.Id, request.LeagueCreateDto);
        }
    }
}
