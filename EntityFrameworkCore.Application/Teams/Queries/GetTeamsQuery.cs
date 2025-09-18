using EntityFrameworkCore.Application.Dtos;
using MediatR;

namespace EntityFrameworkCore.Application.Teams.Queries
{
    public record GetTeamsQuery : IRequest<IEnumerable<TeamReadDto>>;
}
