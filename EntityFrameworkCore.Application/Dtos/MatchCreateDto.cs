using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Application.Dtos
{
    public class MatchCreateDto
    {
        public required string HomeTeamName { get; set; }
        public required string AwayTeamName { get; set; }
        public decimal TicketPrice { get; set; }
        public DateTime Date { get; set; }
    }
}
