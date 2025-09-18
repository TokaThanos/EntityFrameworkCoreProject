using EntityFrameworkCore.Application.Coaches.Commands;
using EntityFrameworkCore.Application.Interfaces;
using MediatR;

namespace EntityFrameworkCore.Application.Coaches.Handlers
{
    public class UpdateCoachHandler : CoachHandlerBase, IRequestHandler<UpdateCoachCommand>
    {
        public UpdateCoachHandler(ICoachService coachService) : base(coachService) { }
        public async Task Handle(UpdateCoachCommand request, CancellationToken cancellationToken)
        {
            await _coachService.UpdateCoachAsync(request.Id, request.CoachCreateDto);
        }
    }
}
