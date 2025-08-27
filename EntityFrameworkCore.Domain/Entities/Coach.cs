namespace EntityFrameworkCore.Domain.Entities;

public class Coach : BaseDomainModel
{
    public string Name { get; set; } = string.Empty;
    public Team? Team { get; set; }
}