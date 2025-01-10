namespace keycontrol.Application.Authentication.Requests;

public record RegisterRequest(string Name, string Email, string Password);