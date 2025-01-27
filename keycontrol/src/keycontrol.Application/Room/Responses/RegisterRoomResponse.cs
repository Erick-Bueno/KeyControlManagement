namespace keycontrol.Application.Room.Responses
{
    public record RegisterRoomResponse(Guid ExternalId, string Name)
        : GlobalResponse("Room registered successfully");
}