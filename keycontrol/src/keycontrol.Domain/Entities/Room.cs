namespace keycontrol.Domain.Entities;
public class Room : Entity
{
    public string Name { get; private set; }
    public List<Key> Keys { get; private set; }
}