using EntityFrameworkCore.Application.Coaches.Queries;
using EntityFrameworkCore.Application.Dtos;
using EntityFrameworkCore.Application.Interfaces;
using MediatR;
namespace EntityFrameworkCore.Application.Coaches.Handlers
{
    public class GetCoachByIdHandler : CoachHandlerBase, IRequestHandler<GetCoachByIdQuery, CoachReadInfoDto?>
    {
        public GetCoachByIdHandler(ICoachService coachService) : base(coachService) { }
        public async Task<CoachReadInfoDto?> Handle(GetCoachByIdQuery request, CancellationToken cancellationToken)
        {
            return await _coachService.GetCoachByIdAsync(request.Id);
        }
    }
}
