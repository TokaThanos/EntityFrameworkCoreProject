using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Application.Dtos
{
    public record LeagueReadInfoDto
    {
        public string LeagueName { get; set; } = string.Empty;
        public List<TeamReadDto>? Teams { get; set; }
    }
}
