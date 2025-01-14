namespace keycontrol.Application.Key.Requests;

public record RegisterKeyRequest(Guid ExternalIdRoom, string Description);