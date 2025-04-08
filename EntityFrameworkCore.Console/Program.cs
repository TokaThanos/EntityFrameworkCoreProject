using EntityFrameworkCore.Console;
using EntityFrameworkCore.Data;
using EntityFrameworkCore.Domain;
using Microsoft.EntityFrameworkCore;

using FootballLeagueDbContext context = new FootballLeagueDbContext();

// await GetAllTeams();
//await GetOneTeam();
//await GetMoreThanOneTeam();
//await AggregateMethods();
//await GroupByMethod();
//await OrderByMethod();
//await SkipAndTake();
//await GroupByWithProjection();

//Console.WriteLine("Enter 0 or 1 to test");
//int option = Convert.ToInt32(Console.ReadLine());
//Console.WriteLine("Enter true or false for query optimization");
//bool optimizationCheck = Convert.ToBoolean(Console.ReadLine());

//await PerformanceHackWithIQueryable(option, optimizationCheck);

async Task GetAllTeams()
{
    var teams = await context.Teams
        .Select(team => new { team.Name, team.CreatedDate }).Take(5)
        .ToListAsync();

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

async Task GetMoreThanOneTeam()
{
    var filteredTeams = await context.Teams.Where(team => team.Id > 1).ToListAsync();
    foreach (var team in filteredTeams)
    {
        Console.WriteLine(team.Name);
    }

    var fcTeams = await context.Teams.Where(team => EF.Functions.Like(team.Name, "%F.C.%")).ToListAsync();
    foreach (var team in fcTeams)
    {
        Console.WriteLine(team.Name);
    }
}

async Task AggregateMethods()
{
    // count
    var teamCount = await context.Teams.CountAsync();

    // max & min
    var maxTeamId = await context.Teams.MaxAsync(team => team.Id);
    var minTeamId = await context.Teams.MinAsync(team => team.Id);

    // average & sum
    var avg = await context.Teams.AverageAsync(team => team.Id);
    var sum = await context.Teams.SumAsync(team => team.Id);

    Console.WriteLine($"{teamCount}, {maxTeamId}, {minTeamId}, {avg}, {sum}");
}

async Task GroupByMethod()
{
    var groupedTeams = context.Teams
        .GroupBy(team => team.CreatedDate.Date);

    foreach (var group in groupedTeams)
    {
        Console.WriteLine(group.Key);
        Console.WriteLine(group.Sum(team => team.Id));
        foreach (var team in group)
        {
            Console.WriteLine(team.Name);
        }
    }
}

async Task OrderByMethod()
{
    var team = await context.Teams.OrderByDescending(team => team.Name).FirstOrDefaultAsync();
    Console.WriteLine(team.Name);
}

async Task SkipAndTake()
{
    var recordCount = 3;
    var page = 0;
    var next = true;
    while (next)
    {
        var teams = await context.Teams.Skip(page * recordCount).Take(recordCount).ToListAsync();
        foreach (var team in teams)
        {
            Console.WriteLine(team.Name);
        }
        if (page == 5)
        {
            break;
        }
        page++;
    }
}

async Task GroupByWithProjection()
{
    var groupedTeams = await context.Teams.
        GroupBy(team => team.CreatedDate.Date)
        .Select(group =>  new TeamInfoDTO { CreatedDate = group.Key, TeamCount = group.Count() }).ToListAsync();

    foreach (TeamInfoDTO team in groupedTeams)
    {
        Console.WriteLine($"{team.TeamCount} teams were created on {team.CreatedDate.Date.ToShortDateString()}");
    }
}

// List vs IQueryable - Performance Hack for EF Core
async Task PerformanceHackWithIQueryable(int choice, bool queryOptimizationCheck)
{
    List<Team> teamsList = new List<Team>();
    // List
    if (!queryOptimizationCheck)
    {
        teamsList = await context.Teams.ToListAsync(); // Query Executing method

        if (choice == 0)
        {
            teamsList = teamsList.Where(team => team.Id == 1).ToList();
        }
        else if (choice == 1)
        {
            teamsList = teamsList.Where(team => team.Name.Contains("F.C.")).ToList();

            // It will throw error as the query has switched to client-evaluation
            // teamsList = teamsList.Where(team => EF.Functions.Like(team.Name, "%F.C.%")).ToList(); 
        }

        foreach (var team in teamsList)
        {
            Console.WriteLine(team.Name);
        }
    }
    // IQueryable
    else
    {
        var teamsAsQueryable = context.Teams.AsQueryable();

        if (choice == 0)
        {
            teamsAsQueryable = teamsAsQueryable.Where(team => team.Id == 1);
        }
        else if (choice == 1)
        {
            teamsAsQueryable = teamsAsQueryable.Where(team => EF.Functions.Like(team.Name, "%F.C.%"));
        }

        teamsList = await teamsAsQueryable.ToListAsync(); // Query Executing method

        foreach (var team in teamsList)
        {
            Console.WriteLine(team.Name);
        }
    }
}