using EntityFrameworkCore.Application.Dtos;
using MediatR;

namespace EntityFrameworkCore.Application.Teams.Queries
{
    public record GetTeamByIdQuery(int Id) : IRequest<TeamReadInfoDto?>;
}
