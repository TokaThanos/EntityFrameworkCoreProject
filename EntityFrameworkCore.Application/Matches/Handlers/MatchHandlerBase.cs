using EntityFrameworkCore.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Application.Matches.Handlers
{
    public abstract class MatchHandlerBase
    {
        protected readonly IMatchService _matchService;

        protected MatchHandlerBase(IMatchService matchService)
        {
            _matchService = matchService;
        }
    }
}
