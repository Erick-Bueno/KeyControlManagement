namespace keycontrol.Application.Rooms.Responses
{
    public record RegisterRoomResponse(Guid ExternalId, string Name)
        : GlobalResponse("Room registered successfully");
}