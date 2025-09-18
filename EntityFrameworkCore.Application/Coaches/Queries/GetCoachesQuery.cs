using EntityFrameworkCore.Application.Dtos;
using MediatR;

namespace EntityFrameworkCore.Application.Coaches.Queries
{
    public record GetCoachesQuery : IRequest<IEnumerable<CoachReadDto>>;
}
