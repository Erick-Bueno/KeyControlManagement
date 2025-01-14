using System.Collections.ObjectModel;

namespace keycontrol.Domain.Entities;

public class KeyRoom : Entity
{   
    public int IdRoom { get; }
    public string? Description { get; }
    public Room? Room { get; }
    public Collection<Report>? Reports { get; }

    public KeyRoom(int idRoom, string? description)
    {
        this.IdRoom = idRoom; 
        this.Description = description;
    }
}