using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Application.Dtos
{
    public record CoachReadInfoDto
    {
        public string CoachName { get; init; } = string.Empty;
        public string? TeamName { get; set; }
    }
}
