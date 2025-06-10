using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Application.Dtos
{
    public class TeamCreateDto
    {
        public string TeamName { get; set; } = string.Empty;
        public string CoachName { get; set; } = string.Empty;
        public string? LeagueName { get; set; }
    }
}
