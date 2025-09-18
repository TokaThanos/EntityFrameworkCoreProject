using EntityFrameworkCore.Application.Dtos;
using MediatR;

namespace EntityFrameworkCore.Application.Leagues.Commands
{
    public record UpdateLeagueCommand(int Id, LeagueCreateDto LeagueCreateDto) : IRequest;
}
