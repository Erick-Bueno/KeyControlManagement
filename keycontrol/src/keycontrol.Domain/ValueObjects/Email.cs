using keycontrol.Domain.Shared;
using System.Text.RegularExpressions;

namespace keycontrol.Domain.ValueObjects;
public record Email
{
    public string EmailValue { get; set; }
    private Email(string emailValue)
    {
        EmailValue = emailValue;
    }
    public static Result<Email> Create(string emailValue)
    {
        if (string.IsNullOrEmpty(emailValue))
        {
            return Result<Email>.Failure("inform an email");
        }
        if (string.IsNullOrWhiteSpace(emailValue))
        {
            return Result<Email>.Failure("inform an email");
        }
        if(!IsValidEmail(emailValue)){
             return Result<Email>.Failure("Invalid email");
        }
        return Result<Email>.Success(new Email(emailValue));
    }
    private static bool IsValidEmail(string email)
    {
        return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }
}