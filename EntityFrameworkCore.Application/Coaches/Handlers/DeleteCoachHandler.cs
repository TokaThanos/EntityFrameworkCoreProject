using EntityFrameworkCore.Application.Coaches.Commands;
using EntityFrameworkCore.Application.Interfaces;
using MediatR;

namespace EntityFrameworkCore.Application.Coaches.Handlers
{
    public class DeleteCoachHandler : CoachHandlerBase, IRequestHandler<DeleteCoachCommand>
    {
        public DeleteCoachHandler(ICoachService coachService) : base(coachService) { }
        public async Task Handle(DeleteCoachCommand request, CancellationToken cancellationToken)
        {
            await _coachService.DeleteCoachByIdAsync(request.Id);
        }
    }
}
