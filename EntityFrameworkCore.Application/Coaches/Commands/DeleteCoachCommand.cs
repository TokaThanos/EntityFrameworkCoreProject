using MediatR;

namespace EntityFrameworkCore.Application.Coaches.Commands
{
    public record DeleteCoachCommand(int Id) : IRequest;
}
