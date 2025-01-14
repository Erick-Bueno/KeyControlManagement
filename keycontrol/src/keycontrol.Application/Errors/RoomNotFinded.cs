using keycontrol.Domain.Enums;

namespace keycontrol.Application.Errors;

public record RoomNotFinded(string Detail):AppError(Detail, nameof(RoomNotFinded), TypeError.Conflict.ToString());