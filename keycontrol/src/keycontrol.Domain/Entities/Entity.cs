using keycontrol.Domain.ValueObjects;

namespace keycontrol.Domain.Entities;

public abstract class Entity
{
    public int Id { get; private set; }
    public Guid ExternalId { get; private set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; private set; }
    
}
