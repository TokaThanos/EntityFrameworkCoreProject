using EntityFrameworkCore.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
