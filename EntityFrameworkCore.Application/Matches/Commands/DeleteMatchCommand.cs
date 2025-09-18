using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Application.Matches.Commands
{
    public record DeleteMatchCommand(int Id) : IRequest;
}
