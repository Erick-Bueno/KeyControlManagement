using keycontrol.Domain.Enums;

namespace keycontrol.Domain.Entities;

public class Report : Entity
{
    public int IdUser { get; }
    public int IdKey { get; }
    public Status Status { get; }
    public DateTime WithdrawalDate { get; }
    public DateTime? ReturnDate { get; }
    public required Key Key { get; set; }
    public required User User { get; set; }
    public Report()
    {
        Status = Status.Available;
    }
}