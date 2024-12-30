using keycontrol.Application.Authentication.Common.Interfaces.Cryptography;

namespace keycontrol.Infrastructure.Cryptography;

public class Bcrypt : IBcrypt
{
    public string EncryptPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string password, string encryptPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, encryptPassword);
    }
}