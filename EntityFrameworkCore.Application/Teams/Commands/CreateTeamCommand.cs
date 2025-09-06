using EntityFrameworkCore.Application.Dtos;
using MediatR;

namespace EntityFrameworkCore.Application.Teams.Commands
{
    public record CreateTeamCommand(TeamCreateDto TeamCreateDto) : IRequest<TeamReadDto>;
}
