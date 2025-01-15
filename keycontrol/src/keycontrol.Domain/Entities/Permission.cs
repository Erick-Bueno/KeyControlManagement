using keycontrol.Domain.Entities;

namespace keycontrol.Domain.Entities;

public class Permission
{
    public int Id { get; set; }
    public string Name { get; init; } = string.Empty;
    public Guid ExternalId { get; protected set; } = Guid.Parse("9e4f49fe-0786-44c6-9061-53d2aa84fab3");
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; protected set; }

}