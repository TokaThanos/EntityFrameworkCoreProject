using MediatR;

namespace EntityFrameworkCore.Application.Teams.Commands
{
    public record DeleteTeamCommand(int Id) : IRequest;
}
