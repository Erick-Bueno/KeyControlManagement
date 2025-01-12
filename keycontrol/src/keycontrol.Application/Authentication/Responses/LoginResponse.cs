namespace keycontrol.Application.Authentication.Responses;

public record LoginResponse(Guid ExternalId, string Name, string Email, string AccessToken, string RefreshToken) : GlobalResponse("User authentication success");