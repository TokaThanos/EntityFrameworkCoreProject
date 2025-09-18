using EntityFrameworkCore.Application.Dtos;
using EntityFrameworkCore.Application.Interfaces;
using EntityFrameworkCore.Data;
using EntityFrameworkCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.Application.Services
{
    public class LeagueService : ILeagueService
    {
        private readonly FootballLeagueDbContext _context;

        public LeagueService(FootballLeagueDbContext context)
        {
            _context = context;
        }
        public async Task<LeagueReadDto> AddLeagueAsync(LeagueCreateDto leagueCreateDto)
        {
            if (string.IsNullOrWhiteSpace(leagueCreateDto.LeagueName))
            {
                throw new ArgumentException("League name can't be null");
            }

            League league = new League
            {
                Name = leagueCreateDto.LeagueName,
            };

            _context.Leagues.Add(league);

            await _context.SaveChangesAsync();

            var leagueReadDto = new LeagueReadDto
            {
                Id = league.Id,
                Name = league.Name
            };

            return leagueReadDto;
        }

        public async Task DeleteLeagueByIdAsync(int id)
        {
            await _context.Leagues.Where(league => league.Id == id).ExecuteDeleteAsync();
        }

        public async Task<IEnumerable<LeagueReadDto>> GetAllLeaguesAsync()
        {
            var leagues = await _context.Leagues
                .Select(league => new LeagueReadDto
                {
                    Id = league.Id,
                    Name = league.Name
                }).ToListAsync();

            return leagues;
        }

        public async Task<LeagueReadInfoDto?> GetLeagueByIdAsync(int id)
        {
            var league = await _context.Leagues
                .Where(league => league.Id == id)
                .Select(league => new LeagueReadInfoDto
                {
                    LeagueName = league.Name,
                    Teams = league.Teams
                        .Select(team => new TeamReadDto
                        {
                            Id = team.Id,
                            Name = team.Name
                        }).ToList()
                })
                .FirstOrDefaultAsync();

            return league;
        }

        public async Task UpdateLeagueAsync(int id, LeagueCreateDto leagueCreateDto)
        {
            if (string.IsNullOrWhiteSpace(leagueCreateDto.LeagueName))
            {
                throw new ArgumentException("League Name can't be null");
            }

            var rowsAffected = await _context.Leagues
                .Where(league => league.Id == id)
                .ExecuteUpdateAsync(league => league
                    .SetProperty(prop => prop.Name, leagueCreateDto.LeagueName));

            if (rowsAffected == 0)
            {
                throw new KeyNotFoundException($"League with ID {id} not found.");
            }
        }
    }
}
