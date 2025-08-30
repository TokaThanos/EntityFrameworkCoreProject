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
    public class GetMatchByIdHandler : MatchHandlerBase, IRequestHandler<GetMatchByIdQuery, MatchReadInfoDto?>
    {
        public GetMatchByIdHandler(IMatchService matchService) : base(matchService) { }

        public async Task<MatchReadInfoDto?> Handle(GetMatchByIdQuery request, CancellationToken cancellationToken)
        {
            return await _matchService.GetMatchByIdAsync(request.Id);
        }
    }
}
