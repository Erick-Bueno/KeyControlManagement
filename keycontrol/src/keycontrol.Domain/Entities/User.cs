namespace keycontrol.Domain.Entities;

public class User : Entity
{
    public string Name { get; init;}
    public string Email { get; init;}
    public string Password { get; init;}
    public List<Report> Reports { get; set; }

    public User(string name, string email, string password)
    {
        this.Name = name;
        this.Email = email;
        this.Password = password;
    }
}