namespace EntityFrameworkCore.Domain.Entities;

public class Coach : BaseDomainModel
{
    public required string Name { get; set; }
    public Team? Team { get; set; }
}