namespace keycontrol.Domain.Entities;

public abstract class Entity
{
    public int Id { get; protected set; }
    public Guid ExternalId { get; protected set; } = Guid.Parse("9e4f49fe-0786-44c6-9061-53d2aa84fab3");
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; protected set; }
    
}