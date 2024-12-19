namespace keycontrol.Application.Authentication.Common.Interfaces.Cryptography;

public interface IBcrypt
{
    public string EncryptPassword(string password);
    public bool DecryptPassword(string password, string encryptPassword);
}