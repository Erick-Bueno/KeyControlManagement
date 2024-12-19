namespace keycontrol.Domain.Entities;

public class User : Entity
{
    public string Name { get; private set;}
    public string Email { get; private set;}
    public string Password { get; private set;}
    public string Iv { get; private set;}
    public List<Report> Reports { get; private set; }
    public User SetName(string name){
        this.Name = name;
        return this;
    }
    public User SetEmail(string email){
        this.Email = email;
        return this;
    }
    public User SetPassword(string password){
        this.Password = password;
        return this;
    }
    public User SetIv(string iv){
        this.Iv = iv;
        return this;
    }
}