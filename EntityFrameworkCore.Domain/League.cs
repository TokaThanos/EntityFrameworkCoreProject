namespace EntityFrameworkCore.Domain;

public class League : BaseDomainModel
{
    public string Name { get; set; } = string.Empty;
    public List<Team> Teams { get; set; } = new List<Team>();
}