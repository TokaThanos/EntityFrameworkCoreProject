using EntityFrameworkCore.Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Application.Matches.Commands
{
    public record CreateMatchCommand(MatchCreateDto MatchCreateDto) : IRequest<MatchReadDto>;
}
