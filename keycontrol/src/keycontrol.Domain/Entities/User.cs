using System.Collections.ObjectModel;

namespace keycontrol.Domain.Entities;

public class User : Entity
{
    public string Name { get; init;}
    public string Email { get; init;}
    public string Password { get; init;}
    public ICollection<Report>? Reports { get; }

    public User(string name, string email, string password)
    {
        Name = name;
        Email = email;
        Password = password;
    }
}