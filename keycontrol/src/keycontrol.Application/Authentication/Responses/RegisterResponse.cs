namespace keycontrol.Application.Authentication.Responses;

public record RegisterResponse(Guid ExternalId, string Name, string Email):GlobalResponse("User registration success");