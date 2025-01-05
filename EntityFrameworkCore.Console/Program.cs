using EntityFrameworkCore.Data;
using Microsoft.EntityFrameworkCore;

using FootballLeagueDbContext context = new FootballLeagueDbContext();

//await GetAllTeams();
//await GetOneTeam();
async Task GetAllTeams()
{
    var teams = await context.Teams.ToListAsync();

    foreach (var team in teams)
    {
        Console.WriteLine(team.Name);
    }
}
async Task GetOneTeam()
{
    // Selects a single record - first one in the list that meets a condition or default value in case of condition is not met
    var team = await context.Teams.FirstOrDefaultAsync(team => team.Id > 1);

    // Selects based on Primary key or the id value
    var teamBasedOnPKey = await context.Teams.FindAsync(2);
}