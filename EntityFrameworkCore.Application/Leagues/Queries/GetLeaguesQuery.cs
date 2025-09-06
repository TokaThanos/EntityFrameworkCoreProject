using EntityFrameworkCore.Application.Dtos;
using MediatR;

namespace EntityFrameworkCore.Application.Leagues.Queries
{
    public record GetLeaguesQuery : IRequest<IEnumerable<LeagueReadDto>>;
}
