using EntityFrameworkCore.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Application.Interfaces
{
    public interface ICoachService
    {
        Task<IEnumerable<CoachReadDto>> GetAllCoachesAsync();
        Task<CoachReadInfoDto?> GetCoachByIdAsync(int id);
        Task<CoachReadDto> AddCoachAsync(CoachCreateDto coachCreateDto);
        Task UpdateCoachAsync(int id, CoachCreateDto coachCreateDto);
        Task DeleteCoachByIdAsync(int id);
    }
}
