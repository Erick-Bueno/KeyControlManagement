namespace keycontrol.Application.Authentication.Responses;

public record RegisterResponse(string ExternalId, string Name, string Email);