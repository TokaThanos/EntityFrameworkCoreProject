using EntityFrameworkCore.Application.Coaches.Queries;
using EntityFrameworkCore.Application.Dtos;
using EntityFrameworkCore.Application.Interfaces;
using MediatR;

namespace EntityFrameworkCore.Application.Coaches.Handlers
{
    public class GetCoachesHandler : CoachHandlerBase, IRequestHandler<GetCoachesQuery, IEnumerable<CoachReadDto>>
    {
        public GetCoachesHandler(ICoachService coachService) : base(coachService) { }
        public Task<IEnumerable<CoachReadDto>> Handle(GetCoachesQuery request, CancellationToken cancellationToken)
        {
            return _coachService.GetAllCoachesAsync();
        }
    }
}
