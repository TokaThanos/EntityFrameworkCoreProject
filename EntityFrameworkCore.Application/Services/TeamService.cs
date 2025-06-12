using EntityFrameworkCore.Application.Dtos;
using EntityFrameworkCore.Application.Interfaces;
using EntityFrameworkCore.Data;
using EntityFrameworkCore.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Application.Services
{
    public class TeamService : ITeamService
    {
        private readonly FootballLeagueDbContext _context;
        public TeamService(FootballLeagueDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TeamReadDto>> GetAllTeamsAsync()
        {
            var teams = await _context.Teams
                .Select(team => new TeamReadDto
                {
                    Id = team.Id,
                    Name = team.Name
                })
                .ToListAsync();

            return teams;
        }

        public async Task<TeamReadInfoDto?> GetTeamByIdAsync(int id)
        {
            var team = await _context.Teams
                .Where(team => team.Id == id)
                .Select(team => new TeamReadInfoDto
                {
                    TeamName = team.Name,
                    CoachName = team.Coach.Name!,
                    LeagueName = team.League.Name
                })
                .FirstOrDefaultAsync();

            return team;
        }

        public async Task DeleteTeamByIdAsync(int id)
        {
            await _context.Teams.Where(team => team.Id == id).ExecuteDeleteAsync();
        }

        public async Task<TeamReadDto> AddTeamAsync(TeamCreateDto teamCreateDto)
        {
            if (string.IsNullOrWhiteSpace(teamCreateDto.TeamName))
            {
                throw new ArgumentException("Team Name can't be null");
            }
            var coach = await _context.Coaches
                .FirstOrDefaultAsync(coach => coach.Name!.ToLower() == teamCreateDto.CoachName.ToLower());

            if (coach == null)
            {
                coach = new Coach { Name = teamCreateDto.CoachName };
                _context.Coaches.Add(coach);
            }
            else
            {
                _context.Attach(coach);
            }

            League? league = null;

            if (!string.IsNullOrWhiteSpace(teamCreateDto.LeagueName))
            {
                league = await _context.Leagues
                    .FirstOrDefaultAsync(league => league.Name.ToLower() == teamCreateDto.LeagueName.ToLower());
                if (league != null)
                {
                    _context.Attach(league);
                }
                else
                {
                    throw new ArgumentException($"League {teamCreateDto.LeagueName} not found.");
                }
            }

            Team team = new Team
            {
                Name = teamCreateDto.TeamName,
                Coach = coach,
                League = league
            };

            _context.Teams.Add(team);
            await _context.SaveChangesAsync();

            var teamInfo = new TeamReadDto
            {
                Id = team.Id,
                Name = team.Name
            };

            return teamInfo;
        }

        public async Task UpdateTeamAsync(int id, TeamCreateDto newTeam)
        {
            var team = await _context.Teams
                .AsTracking()
                .FirstOrDefaultAsync(team => team.Id == id);

            if (team == null)
            {
                throw new KeyNotFoundException($"Team with ID {id} not found.");
            }

            if (!string.IsNullOrWhiteSpace(newTeam.TeamName))
            {
                team.Name = newTeam.TeamName;
            }

            if (!string.IsNullOrWhiteSpace(newTeam.CoachName))
            {
                var coach = await _context.Coaches
                    .FirstOrDefaultAsync(coach => coach.Name!.ToLower() ==  newTeam.CoachName.ToLower());
                if (coach == null)
                {
                    coach = new Coach { Name = newTeam.CoachName };
                    _context.Coaches.Add(coach);
                }
                team.Coach = coach;
            }

            if (!string.IsNullOrWhiteSpace(newTeam.LeagueName))
            {
                var league = await _context.Leagues
                    .FirstOrDefaultAsync(league => league.Name.ToLower() == newTeam.LeagueName.ToLower());
                if (league == null)
                {
                    throw new ArgumentException($"League {newTeam.LeagueName} not found.");
                }
                team.League = league;
            }
            await _context.SaveChangesAsync();
        }
    }
}
