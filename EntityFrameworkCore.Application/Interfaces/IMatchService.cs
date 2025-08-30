using EntityFrameworkCore.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Application.Interfaces
{
    public interface IMatchService
    {
        Task<IEnumerable<MatchReadDto>> GetAllMatchesAsync();
        Task<MatchReadInfoDto?> GetMatchByIdAsync(int id);
        Task DeleteMatchByIdAsync(int id);
        Task<MatchReadDto> AddMatchAsync(MatchCreateDto match);
        Task UpdateMatchAsync(int id, MatchUpdateDto match);
    }
}
