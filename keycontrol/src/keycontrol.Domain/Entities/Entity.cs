namespace keycontrol.Domain.Entities;

public abstract class Entity
{
    public int Id { get; protected set; }
    public Guid ExternalId { get; protected set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; protected set; }
    public DateTime UpdatedAt { get; protected set; }
}