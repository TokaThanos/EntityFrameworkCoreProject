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
    public class UpdateMatchHandler : MatchHandlerBase, IRequestHandler<UpdateMatchCommand>
    {
        public UpdateMatchHandler(IMatchService matchService) : base(matchService) { }

        public async Task Handle(UpdateMatchCommand request, CancellationToken cancellationToken)
        {
            await _matchService.UpdateMatchAsync(request.Id, request.MatchUpdateDto);
        }
    }
}
