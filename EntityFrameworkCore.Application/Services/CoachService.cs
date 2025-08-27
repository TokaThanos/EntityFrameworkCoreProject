using EntityFrameworkCore.Application.Dtos;
using EntityFrameworkCore.Application.Interfaces;
using EntityFrameworkCore.Data;
using EntityFrameworkCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Application.Services
{
    public class CoachService : ICoachService
    {
        private readonly FootballLeagueDbContext _context;

        public CoachService(FootballLeagueDbContext context)
        {
            _context = context;
        }
        public async Task<CoachReadDto> AddCoachAsync(CoachCreateDto coachCreateDto)
        {
            if (string.IsNullOrWhiteSpace(coachCreateDto.CoachName))
            {
                throw new ArgumentException("Coach name can't be null");
            }

            Coach coach = new Coach();

            coach.Name = coachCreateDto.CoachName;
            _context.Coaches.Add(coach);

            await _context.SaveChangesAsync();

            var coachReadDto = new CoachReadDto
            {
                Id = coach.Id,
                Name = coach.Name
            };

            return coachReadDto;
        }

        public async Task DeleteCoachByIdAsync(int id)
        {
            await _context.Coaches.Where(coach => coach.Id == id).ExecuteDeleteAsync();
        }

        public async Task<IEnumerable<CoachReadDto>> GetAllCoachesAsync()
        {
            var coaches = await _context.Coaches
                .Select(coach => new CoachReadDto
                {
                    Id = coach.Id,
                    Name = coach.Name
                })
                .ToListAsync();

            return coaches;
        }

        public async Task<CoachReadInfoDto?> GetCoachByIdAsync(int id)
        {
            var coach = await _context.Coaches
                .Where(coach => coach.Id == id)
                .Select(coach => new CoachReadInfoDto
                {
                    CoachName = coach.Name,
                    TeamName = coach.Team != null ? coach.Team.Name : null
                })
                .FirstOrDefaultAsync();

            return coach;
        }

        public async Task UpdateCoachAsync(int id, CoachCreateDto coachCreateDto)
        {
            var coach = await _context.Coaches
                .AsTracking()
                .FirstOrDefaultAsync(coach => coach.Id == id);

            if (coach == null)
            {
                throw new KeyNotFoundException($"Coach with ID {id} not found.");
            }

            if (string.IsNullOrWhiteSpace(coachCreateDto.CoachName)) 
            {
                throw new ArgumentException("Coach Name can't be null");
            }

            coach.Name = coachCreateDto.CoachName;
            await _context.SaveChangesAsync();
        }
    }
}
