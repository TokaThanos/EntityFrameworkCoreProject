using EntityFrameworkCore.Application.Dtos;
using EntityFrameworkCore.Application.Interfaces;
using EntityFrameworkCore.Application.Matches.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Application.Matches.Handlers
{
    public class GetMatchesHandler : MatchHandlerBase, IRequestHandler<GetMatchesQuery, IEnumerable<MatchReadDto>>
    {
        public GetMatchesHandler(IMatchService matchService) : base(matchService) { }

        public Task<IEnumerable<MatchReadDto>> Handle(GetMatchesQuery request, CancellationToken cancellationToken)
        {
            return _matchService.GetAllMatchesAsync();
        }
    }
}
