namespace keycontrol.Application.Reports.Responses;

public record ReturnKeyResponse(Guid ExternalIdUser, Guid ExternalIdKey, string Name, DateTime WithdrawalDate, DateTime ReturnDate) : GlobalResponse("Return key successfully");