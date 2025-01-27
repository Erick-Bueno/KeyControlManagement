namespace keycontrol.Application.Keys.Requests;

public record RegisterKeyRequest(Guid ExternalIdRoom, string Description);