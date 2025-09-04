using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Application.Coaches.Commands
{
    public record DeleteCoachCommand(int Id) : IRequest;
}
