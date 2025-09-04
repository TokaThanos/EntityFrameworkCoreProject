using EntityFrameworkCore.Application.Dtos;
using MediatR;

namespace EntityFrameworkCore.Application.Matches.Queries
{
    public record GetMatchesQuery : IRequest<IEnumerable<MatchReadDto>>;
}
