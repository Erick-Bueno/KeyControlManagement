using keycontrol.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace keycontrol.Infrastructure.Authentication;

public sealed class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(Permission permission) : base(policy: permission.ToString())
    {
        
    }
}