using keycontrol.Domain.Shared;
using keycontrol.Domain.ValueObjects;
using System.Collections.ObjectModel;

namespace keycontrol.Domain.Entities;

public class User : Entity
{

    public string Name { get; private set; }
    public Email Email { get; private set; }
    public string Password { get; private set; }
    public ICollection<Report>? Reports { get; }

    private User(string name, Email email, string password)
    {
        Name = name;
        Email = email;
        Password = password;
    }

   
    public static Result<User> Create(string name, Email email, string password)
    {
        if (string.IsNullOrEmpty(name))
        {
            return Result<User>.Failure("Inform an name");
        }
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result<User>.Failure("Inform an name");
        }
        return Result<User>.Success(new User(name, email, password));
    }
}
