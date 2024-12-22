namespace keycontrol.Application.Authentication.Common.Interfaces.Authentication;

public interface ITokenJwtGenerator
{
   public string GenerateAccessToken(Guid userId);
   public string GenerateRefreshToken(); 
}