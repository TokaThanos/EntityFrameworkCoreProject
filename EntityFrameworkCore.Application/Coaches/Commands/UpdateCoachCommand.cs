using EntityFrameworkCore.Application.Dtos;
using MediatR;

namespace EntityFrameworkCore.Application.Coaches.Commands
{
    public record UpdateCoachCommand(int Id, CoachCreateDto CoachCreateDto) : IRequest;
}
