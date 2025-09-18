using EntityFrameworkCore.Application.Coaches.Commands;
using EntityFrameworkCore.Application.Dtos;
using EntityFrameworkCore.Application.Interfaces;
using MediatR;

namespace EntityFrameworkCore.Application.Coaches.Handlers
{
    public class CreateCoachHandler : CoachHandlerBase, IRequestHandler<CreateCoachCommand, CoachReadDto>
    {
        public CreateCoachHandler(ICoachService coachService) : base(coachService) { }
        public async Task<CoachReadDto> Handle(CreateCoachCommand request, CancellationToken cancellationToken)
        {
            return await _coachService.AddCoachAsync(request.CoachCreateDto);
        }
    }
}
