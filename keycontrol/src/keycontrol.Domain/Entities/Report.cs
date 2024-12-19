using keycontrol.Domain.Enums;

namespace keycontrol.Domain.Entities;

public class Report : Entity
{
    public int IdUser { get; private set; }
    public int IdKey { get; private set; }
    public Status Status { get; private set; }
    public DateTime WithdrawalDate { get; private set; }
    public DateTime? ReturnDate { get; private set; }
    public Key Key { get; private set; }
    public User User { get; private set; }
    public Report(){
        Status = Status.Available;
    }
}