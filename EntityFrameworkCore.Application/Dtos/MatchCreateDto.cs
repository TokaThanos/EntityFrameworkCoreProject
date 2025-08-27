using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Application.Dtos
{
    public class MatchCreateDto
    {
        public string HomeTeamName { get; set; } = string.Empty;
        public string AwayTeamName { get; set; } = string.Empty;
        public decimal TicketPrice { get; set; }
        public DateTime Date { get; set; }
    }
}
