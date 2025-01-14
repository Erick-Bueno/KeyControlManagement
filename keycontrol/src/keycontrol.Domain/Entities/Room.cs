using System.Collections.ObjectModel;

namespace keycontrol.Domain.Entities;
public class Room : Entity
{
    public string? Name { get; }
    public Collection<KeyRoom>? Keys { get;}
}