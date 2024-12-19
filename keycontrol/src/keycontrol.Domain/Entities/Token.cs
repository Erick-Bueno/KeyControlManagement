namespace keycontrol.Domain.Entities;

public class Token : Entity
{
    public string Email { get; private set; }
    public string RefreshToken { get; private set; }
}