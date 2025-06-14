using EntityFrameworkCore.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Application.Interfaces
{
    public interface ILeagueService
    {
        Task<IEnumerable<LeagueReadDto>> GetAllLeaguesAsync();
        Task<LeagueReadInfoDto?> GetLeagueByIdAsync(int id);
        Task<LeagueReadDto> AddLeagueAsync(LeagueCreateDto leagueCreateDto);
        Task DeleteLeagueByIdAsync(int id);
        Task UpdateLeagueAsync(int id, LeagueCreateDto leagueCreateDto);
    }
}
