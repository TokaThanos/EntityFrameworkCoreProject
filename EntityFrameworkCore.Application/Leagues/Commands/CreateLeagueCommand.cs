using EntityFrameworkCore.Application.Dtos;
using MediatR;

namespace EntityFrameworkCore.Application.Leagues.Commands
{
    public record CreateLeagueCommand(LeagueCreateDto LeagueCreateDto) : IRequest<LeagueReadDto>;
}
