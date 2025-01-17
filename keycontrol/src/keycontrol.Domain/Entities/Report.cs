using keycontrol.Domain.Enums;
using keycontrol.Domain.ValueObjects;

namespace keycontrol.Domain.Entities;

public class Report : Entity
{
    public int IdUser { get; private set;}
    public int IdKey { get; private set;}
    public Status Status { get; private set;}
    public DateTime WithdrawalDate { get; private set;}
    public DateTime? ReturnDate { get; private set;}
    public  KeyRoom Key { get;}
    public  User User { get;}
    public Report()
    {
        Status = Status.Available;
    }
    public static Report Create(User user, KeyRoom key){
        var report = new Report{
            IdKey = key.Id,
            IdUser = user.Id,
        };
        return report;
    }
}