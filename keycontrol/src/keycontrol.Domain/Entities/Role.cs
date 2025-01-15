using keycontrol.Domain.Enums;
using System.Collections.ObjectModel;

namespace keycontrol.Domain.Entities;

public sealed class Role : Enumeration<Role>
{
    public static readonly Role Doorman = new Role(1, "Doorman");
    public static readonly Role CommonUser = new Role(2, "CommonUser");

    public Role(int id, string name) : base(id, name)
    {
    }

    public ICollection<Permission>? Permissions { get; }
    public ICollection<User>? Users { get; }
}