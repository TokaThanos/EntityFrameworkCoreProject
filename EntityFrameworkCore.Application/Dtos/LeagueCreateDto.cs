using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Application.Dtos
{
    public class LeagueCreateDto
    {
        public required string LeagueName { get; set; }
    }
}
