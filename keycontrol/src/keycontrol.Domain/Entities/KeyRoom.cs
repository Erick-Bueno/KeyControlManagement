using System.Collections.ObjectModel;

namespace keycontrol.Domain.Entities;

public class KeyRoom : Entity
{   
    public int IdRoom { get; init; }
    public string? Description { get; init; }
    public Room? Room { get; }
    public ICollection<Report>? Reports { get; }
    
    public KeyRoom(int idRoom, string? description)
    {
        IdRoom = idRoom; 
        Description = description;
    }
}