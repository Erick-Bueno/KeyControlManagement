using System.Collections.ObjectModel;

namespace keycontrol.Domain.Entities;

public class Key : Entity
{   
    public int IdRoom { get; }
    public Room? Room { get; }
    public Collection<Report>? Reports { get; }
}