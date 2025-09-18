using MediatR;

namespace EntityFrameworkCore.Application.Leagues.Commands
{
    public record DeleteLeagueCommand(int Id) : IRequest;
}
