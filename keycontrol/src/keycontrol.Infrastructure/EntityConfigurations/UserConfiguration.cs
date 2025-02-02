using keycontrol.Domain.Entities;
using keycontrol.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace keycontrol.Infrastructure.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.HasIndex(u => u.ExternalId);
        builder.Property(u => u.Name).IsRequired().HasMaxLength(200);
        builder.Property(u => u.blocked).IsRequired();
        builder.Property(u => u.Email).IsRequired()
        .HasMaxLength(200)
        .HasConversion(
            email => email.EmailValue,
            value => Email.Create(value).Value
        );
        builder.Property(u => u.Password)
        .IsRequired()
        .HasMaxLength(400);
    }
}