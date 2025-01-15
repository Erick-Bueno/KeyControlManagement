using keycontrol.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace keycontrol.Infrastructure.EntityConfigurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(b => b.Id);
        builder.HasMany(b => b.Permissions)
            .WithMany()
            .UsingEntity<RolePermission>();
        builder.HasMany(b => b.Users)
            .WithMany();
    }
}