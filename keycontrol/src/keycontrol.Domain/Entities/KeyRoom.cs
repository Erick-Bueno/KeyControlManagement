using keycontrol.Domain.Enums;
using keycontrol.Domain.Shared;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace keycontrol.Domain.Entities;

public class KeyRoom : Entity
{
    public int IdRoom { get; private set; }
    public string? Description { get; private set; }
    public Status Status { get; private set; }
    public Room? Room { get; }
    public ICollection<Report>? Reports { get; }

    private KeyRoom(int idRoom, string? description)
    {
        IdRoom = idRoom;
        Description = description;
        Status = Status.Available;
    }
    public KeyRoom() { }
    public static Result<KeyRoom> Create(int idRoom, string description)
    {
        if (string.IsNullOrEmpty(description))
        {
            return Result<KeyRoom>.Failure("Inform an description");
        }
        if (string.IsNullOrWhiteSpace(description))
        {
            return Result<KeyRoom>.Failure("Inform an description");
        }
        return Result<KeyRoom>.Success(new KeyRoom(idRoom, description));
    }

    public void UpdateStatus(Status newStatus)
    {
        Status = newStatus;
    }

}