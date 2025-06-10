using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Application.Dtos
{
    public record TeamReadInfoDto
    {
        public string TeamName { get; init; } = string.Empty;
        public string CoachName { get; init; } = string.Empty;
        public string? LeagueName { get; init; }
    }
}
