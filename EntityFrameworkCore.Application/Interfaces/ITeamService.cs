using EntityFrameworkCore.Application.Dtos;
using EntityFrameworkCore.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Application.Interfaces
{
    public interface ITeamService
    {
        Task<IEnumerable<TeamReadDto>> GetAllTeamsAsync();
        Task<TeamReadInfoDto?> GetTeamByIdAsync(int id);
        Task DeleteTeamByIdAsync(int id);
        Task<TeamReadDto> AddTeamAsync(TeamCreateDto team);
        Task UpdateTeamAsync(int id, TeamCreateDto team);
    }
}
