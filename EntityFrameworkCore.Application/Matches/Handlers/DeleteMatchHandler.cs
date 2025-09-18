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
    public class DeleteMatchHandler : MatchHandlerBase, IRequestHandler<DeleteMatchCommand>
    {
        public DeleteMatchHandler(IMatchService matchService) : base(matchService) { }

        public async Task Handle(DeleteMatchCommand request, CancellationToken cancellationToken)
        {
            await _matchService.DeleteMatchByIdAsync(request.Id);
        }
    }
}
