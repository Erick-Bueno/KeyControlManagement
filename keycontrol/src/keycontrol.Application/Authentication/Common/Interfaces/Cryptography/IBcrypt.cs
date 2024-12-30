namespace keycontrol.Application.Authentication.Common.Interfaces.Cryptography;

public interface IBcrypt
{
    public string EncryptPassword(string password);
    public bool VerifyPassword(string password, string encryptPassword);
}