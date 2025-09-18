using EntityFrameworkCore.Application.Dtos;
using EntityFrameworkCore.Application.Interfaces;
using EntityFrameworkCore.Application.Leagues.Commands;
using MediatR;

namespace EntityFrameworkCore.Application.Leagues.Handlers
{
    public class CreateLeagueHandler : LeagueHandlerBase, IRequestHandler<CreateLeagueCommand, LeagueReadDto>
    {
        public CreateLeagueHandler(ILeagueService leagueService) : base(leagueService) { }

        public async Task<LeagueReadDto> Handle(CreateLeagueCommand request, CancellationToken cancellationToken)
        {
            return await _leagueService.AddLeagueAsync(request.LeagueCreateDto);
        }
    }
}
