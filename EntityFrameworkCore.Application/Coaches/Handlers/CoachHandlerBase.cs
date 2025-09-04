using EntityFrameworkCore.Application.Interfaces;

namespace EntityFrameworkCore.Application.Coaches.Handlers
{
    public abstract class CoachHandlerBase
    {
        protected readonly ICoachService _coachService;

        protected CoachHandlerBase(ICoachService coachService) 
        {
            _coachService = coachService;
        }
    }
}
