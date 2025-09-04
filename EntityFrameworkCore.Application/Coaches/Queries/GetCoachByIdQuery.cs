

using EntityFrameworkCore.Application.Dtos;
using MediatR;

namespace EntityFrameworkCore.Application.Coaches.Queries
{
    public record GetCoachByIdQuery(int Id) : IRequest<CoachReadInfoDto>;
}
