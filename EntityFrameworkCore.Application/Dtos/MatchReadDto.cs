using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Application.Dtos
{
    public class MatchReadDto
    {
        public int Id { get; set; }
        public string HomeTeamName { get; set; } = string.Empty;
        public string AwayTeamName { get; set; } = string.Empty;
    }
}
