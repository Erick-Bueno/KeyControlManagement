using keycontrol.Application.Errors;
using keycontrol.Application.Reports.Responses;
using keycontrol.Application.Repositories;
using keycontrol.Domain.Entities;
using keycontrol.Domain.Enums;
using MediatR;
using OneOf;
using System.Linq.Expressions;

namespace keycontrol.Application.Reports.Commands.RentKey;

public class RentKeyCommandHandler : IRequestHandler<RentKeyCommand, OneOf<RentKeyResponse, AppError>>
{
    private readonly IUserRepository _userRepository;
    private readonly IKeyRepository _keyRepository;
    private readonly IReportRepository _reportRepository;

    public RentKeyCommandHandler(IUserRepository userRepository, IKeyRepository keyRepository, IReportRepository reportRepository)
    {
        _userRepository = userRepository;
        _keyRepository = keyRepository;
        _reportRepository = reportRepository;
    }

    public async Task<OneOf<RentKeyResponse, AppError>> Handle(RentKeyCommand request, CancellationToken cancellationToken)
    {
        var userIsFinded = await _userRepository.FindUserByExternalId(request.ExternalIdUser);
        if (userIsFinded == null)
        {
            return new UserNotRegistered("User Not Registered");
        }
        if (userIsFinded.blocked)
        {
            return new UserBlocked("This User Is Blocked");
        }
        var key = await _keyRepository.FindKeyByExternalId(request.ExternalIdKey);
        if (key.Status == Status.Unavailable)
        {
            return new KeyUnavailable("This Key Is Unavailable");
        }
        var report = Report.Create(userIsFinded, key);
        await _reportRepository.RentKey(report.Value);
        return new RentKeyResponse(userIsFinded.ExternalId, key.ExternalId, userIsFinded.Name, report.Value.WithdrawalDate);
    }
}