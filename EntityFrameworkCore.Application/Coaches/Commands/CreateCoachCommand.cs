using EntityFrameworkCore.Application.Dtos;
using MediatR;

namespace EntityFrameworkCore.Application.Coaches.Commands
{
    public record CreateCoachCommand(CoachCreateDto CoachCreateDto) : IRequest<CoachReadDto>;
}
