using EntityFrameworkCore.Application.Dtos;
using EntityFrameworkCore.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Application.Services
{
    public class MatchService : IMatchService
    {
        public Task<MatchReadDto> AddMatchAsync(MatchCreateDto match)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMatchById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MatchReadDto>> GetAllMatchesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<MatchReadInfoDto?> GetMatchByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateMatchAsync(int id, MatchCreateDto match)
        {
            throw new NotImplementedException();
        }
    }
}
