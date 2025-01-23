using keycontrol.Domain.Shared;

namespace keycontrol.Domain.Entities;
public class Room : Entity
{
    public string? Name { get; private set; }
    public ICollection<KeyRoom>? Keys { get; private set; }

    private Room(string? name)
    {
        Name = name;
    }
    public static Result<Room> Create(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return Result<Room>.Failure("Inform an name");
        }
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result<Room>.Failure("Inform an name");
        }
        return Result<Room>.Success(new Room(name));
    }
}