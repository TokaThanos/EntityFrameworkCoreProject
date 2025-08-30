using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Application.Dtos
{
    public class TeamCreateDto
    {
        public required string TeamName { get; set; }
        public required string CoachName { get; set; }
        public string? LeagueName { get; set; }
    }
}
