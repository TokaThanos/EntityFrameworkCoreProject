namespace EntityFrameworkCore.Domain.Entities;

public class Team : BaseDomainModel
{
    public required string Name { get; set; }

    public Coach? Coach { get; set; }
    public int CoachId { get; set; }

    public League? League { get; set; }
    public int? LeagueId { get; set; }

    public List<Match> HomeMatches { get; set; } = new List<Match>();
    public List<Match> AwayMatches { get; set; } = new List<Match>();
}