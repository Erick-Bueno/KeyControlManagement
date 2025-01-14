namespace keycontrol.Application.Key.Responses;

public record RegisterKeyResponse(Guid ExternalId, Guid ExternalIdRoom, string Description, string Room)
    : GlobalResponse("Key registered successfully");
