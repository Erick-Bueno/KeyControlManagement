namespace keycontrol.Domain.Entities;

public class RolePermission : Entity
{
    public int RoleId { get; set; }
    public int PermissionId { get; set; }
}