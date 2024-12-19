namespace keycontrol.Domain.Entities;

public class Key : Entity
{   
    public int IdRoom { get; private set; }
    public Room Room { get; private set; }
    public List<Report> Reports { get; private set; }
}