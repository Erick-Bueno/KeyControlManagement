using keycontrol.Domain.Enums;
using keycontrol.Domain.Shared;
using keycontrol.Domain.ValueObjects;

namespace keycontrol.Domain.Entities;

public class Report : Entity
{
    public int IdUser { get; private set;}
    public int IdKey { get; private set;}
    public DateTime WithdrawalDate { get; private set;}
    public DateTime? ReturnDate { get; private set;}
    public  KeyRoom Key { get;}
    public  User User { get;}
    private Report()
    {

    }
    public static Result<Report> Create(User user, KeyRoom key){
        var report = new Report{
            IdKey = key.Id,
            IdUser = user.Id,
        };
        return Result<Report>.Success(report);
    }
}