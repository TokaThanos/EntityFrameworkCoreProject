using EntityFrameworkCore.Application.Dtos;
using EntityFrameworkCore.Application.Interfaces;
using EntityFrameworkCore.Application.Matches.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Application.Matches.Handlers
{
    public class CreateMatchHandler : MatchHandlerBase, IRequestHandler<CreateMatchCommand, MatchReadDto>
    {
        public CreateMatchHandler(IMatchService matchService) : base(matchService) { }
        public async Task<MatchReadDto> Handle(CreateMatchCommand request, CancellationToken cancellationToken)
        {
            return await _matchService.AddMatchAsync(request.MatchCreateDto);
        }
    }
}
