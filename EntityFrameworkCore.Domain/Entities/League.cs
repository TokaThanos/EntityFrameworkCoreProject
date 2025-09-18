namespace EntityFrameworkCore.Domain.Entities;

public class League : BaseDomainModel
{
    public required string Name { get; set; }
    public List<Team> Teams { get; set; } = new List<Team>();
}