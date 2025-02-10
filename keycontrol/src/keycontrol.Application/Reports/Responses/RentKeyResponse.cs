using System.Data.Common;

namespace keycontrol.Application.Reports.Responses;

public record RentKeyResponse(Guid ExternalIdUser, Guid ExternalIdKey, string Name, DateTime WithdrawalDate) : GlobalResponse("Key rented successfully");