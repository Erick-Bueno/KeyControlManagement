namespace keycontrol.Application.Authentication.Responses;

public class RegisterResponse
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }

    public RegisterResponse(Guid id, string name, string email)
    {
        Id = id;
        UserName = name;
        Email = email;
    }
}