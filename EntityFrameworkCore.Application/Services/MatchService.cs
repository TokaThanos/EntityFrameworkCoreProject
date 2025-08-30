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
    public class MatchService : IMatchService
    {
        private readonly FootballLeagueDbContext _context;
        public MatchService(FootballLeagueDbContext context)
        {
            _context = context;
        }

        public async Task<MatchReadDto> AddMatchAsync(MatchCreateDto matchCreateDto)
        {
            var homeTeam = await _context.Teams.FirstOrDefaultAsync(team => team.Name.ToLower() == matchCreateDto.HomeTeamName!.ToLower());
            var awayTeam = await _context.Teams.FirstOrDefaultAsync(team => team.Name.ToLower() == matchCreateDto.AwayTeamName!.ToLower());

            if (homeTeam is null || awayTeam is null)
            {
                throw new ArgumentException("Invalid teams!");
            }
            else
            {
                _context.Attach(homeTeam);
                _context.Attach(awayTeam);
            }

            var match = new Match
            {
                HomeTeam = homeTeam,
                AwayTeam = awayTeam,
                TicketPrice = matchCreateDto.TicketPrice,
                Date = matchCreateDto.Date,
            };

            _context.Matches.Add(match);
            await _context.SaveChangesAsync();

            var matchInfo = new MatchReadDto
            {
                Id = match.Id,
                HomeTeamName = match.HomeTeam.Name,
                AwayTeamName = match.AwayTeam.Name
            };

            return matchInfo;
        }

        public async Task DeleteMatchByIdAsync(int id)
        {
            await _context.Matches.Where(match => match.Id == id).ExecuteDeleteAsync();
        }

        public async Task<IEnumerable<MatchReadDto>> GetAllMatchesAsync()
        {
            var matches = await _context.Matches
                .Include(match => match.HomeTeam)
                .Include(match => match.AwayTeam)
                .Select(match => new MatchReadDto
                {
                    Id = match.Id,
                    HomeTeamName = match.HomeTeam.Name,
                    AwayTeamName = match.AwayTeam.Name
                }).ToListAsync();

            return matches;
        }

        public async Task<MatchReadInfoDto?> GetMatchByIdAsync(int id)
        {
            var match = await _context.Matches
                .Where(match => match.Id == id)
                .Include(team => team.HomeTeam)
                .Include(team => team.AwayTeam)
                .FirstOrDefaultAsync();

            if (match is null)
            {
                return null;
            }

            var matchInfo = new MatchReadInfoDto
            {
                HomeTeamName = match.HomeTeam.Name,
                AwayTeamName = match.AwayTeam.Name,
                HomeTeamScore = match.HomeTeamScore,
                AwayTeamScore = match.AwayTeamScore,
                TicketPrice = match.TicketPrice,
                Date = match.Date,
                Status = match.Status
            };

            return matchInfo;
        }

        public async Task UpdateMatchAsync(int id, MatchUpdateDto matchUpdateDto)
        {
            var match = await _context.Matches
                .AsTracking()
                .FirstOrDefaultAsync(match => match.Id == id);

            if (match is null) 
            {
                throw new KeyNotFoundException($"Match with Id {id} not found!");
            }

            if (!string.IsNullOrEmpty(matchUpdateDto.HomeTeamName))
            {
                var homeTeam = await _context.Teams.FirstOrDefaultAsync(team => team.Name.ToLower() == matchUpdateDto.HomeTeamName.ToLower());
                if (homeTeam is null)
                {
                    throw new ArgumentException("Invalid team!");
                }
                match.HomeTeam = homeTeam;
            }

            if (!string.IsNullOrEmpty(matchUpdateDto.AwayTeamName))
            {
                var awayTeam = await _context.Teams.FirstOrDefaultAsync(team => team.Name.ToLower() == matchUpdateDto.AwayTeamName.ToLower());
                if (awayTeam is null)
                {
                    throw new ArgumentException("Invalid team!");
                }
                match.AwayTeam = awayTeam;
            }

            if (matchUpdateDto.Date.HasValue)
            {
                match.Date = (DateTime)matchUpdateDto.Date;
            }
            if (matchUpdateDto.TicketPrice.HasValue)
            {
                match.TicketPrice = (Decimal)matchUpdateDto.TicketPrice;
            }

            await _context.SaveChangesAsync();
        }
    }
}
