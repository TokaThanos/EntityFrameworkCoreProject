using EntityFrameworkCore.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Application.Dtos
{
    public class MatchReadInfoDto : MatchCreateDto
    {
        public int HomeTeamScore { get; set; }
        public int AwayTeamScore { get; set; }
        public MatchStatus Status { get; set; }
    }
}
