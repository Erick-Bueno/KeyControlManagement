using keycontrol.Application.Errors;
using keycontrol.Application.Reports.Responses;
using keycontrol.Application.Repositories;
using keycontrol.Domain.Enums;
using MediatR;
using OneOf;

namespace keycontrol.Application.Reports.Commands.ReturnKey;

public class ReturnKeyCommandHandler : IRequestHandler<ReturnKeyCommand, OneOf<ReturnKeyResponse, AppError>>
{
    private readonly IUserRepository _userRepository;
    private readonly IKeyRepository _keyRepository;
    private readonly IReportRepository _reportRepository;
    public ReturnKeyCommandHandler(IUserRepository userRepository, IKeyRepository keyRepository, IReportRepository reportRepository)
    {
        _userRepository = userRepository;
        _keyRepository = keyRepository;
        _reportRepository = reportRepository;
    }

    public async Task<OneOf<ReturnKeyResponse, AppError>> Handle(ReturnKeyCommand request, CancellationToken cancellationToken)
    {
        var reportFound = await _reportRepository.FindReportByExternalId(request.ExternalIdReport);
        if(reportFound is null){
            return new ReportNotFound("Report not found");
        }
        var userFound = await _userRepository.FindUserById(reportFound.IdUser);
        if (userFound is null)
        {
            return new UserNotRegistered("User not registered");
        }
        var keyFound = await _keyRepository.FindUserById(reportFound.IdKey);
        if (keyFound is null)
        {
            return new KeyNotFound("Key not found");
        }
        keyFound.UpdateStatus(Status.Available);
        if(userFound is {Blocked: true}){
            userFound.UpdateStatus(false);
        }
        reportFound.UpdateReturnDate(DateTime.Now);
       
    }
}