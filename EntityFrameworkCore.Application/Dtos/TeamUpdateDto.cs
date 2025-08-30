using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Application.Dtos
{
    public class TeamUpdateDto
    {
        public string? TeamName { get; set; }
        public string? CoachName { get; set; }
        public string? LeagueName { get; set; }
    }
}
