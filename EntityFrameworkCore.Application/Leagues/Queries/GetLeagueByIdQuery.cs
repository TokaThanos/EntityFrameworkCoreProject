using EntityFrameworkCore.Application.Dtos;
using MediatR;

namespace EntityFrameworkCore.Application.Leagues.Queries
{
    public record GetLeagueByIdQuery(int Id) : IRequest<LeagueReadInfoDto?>;
}
