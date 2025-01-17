using keycontrol.Domain.Shared;
using System.Text.RegularExpressions;

namespace keycontrol.Domain.ValueObjects;

public record Password
{
    public string PasswordValue { get; set; }
    private Password(string passwordValue)
    {
        PasswordValue = passwordValue;
    }
    public static Result<Password> Create(string passwordValue){
        if(string.IsNullOrEmpty(passwordValue)){
            return Result<Password>.Failure("inform an password");
        }
        if(string.IsNullOrWhiteSpace(passwordValue)){
            return Result<Password>.Failure("Inform an password");
        }
        if(!IsValidPassword(passwordValue)){
            return Result<Password>.Failure("Invalid password ");
        }
        return Result<Password>.Success(new Password(passwordValue));
    }
    private static bool IsValidPassword(string passwordValue){
        return Regex.IsMatch(passwordValue ,@"(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[-_@#$%]).{8,}");
    }

}