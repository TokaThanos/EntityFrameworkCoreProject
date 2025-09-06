using EntityFrameworkCore.Application.Dtos;
using MediatR;

namespace EntityFrameworkCore.Application.Teams.Commands
{
    public record UpdateTeamCommand(int Id, TeamUpdateDto TeamUpdateDto) : IRequest;
}
