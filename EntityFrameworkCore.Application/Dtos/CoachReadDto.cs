using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Application.Dtos
{
    public record CoachReadDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
    }
}
