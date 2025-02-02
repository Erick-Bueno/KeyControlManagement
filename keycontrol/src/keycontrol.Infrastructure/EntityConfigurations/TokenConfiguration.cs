using keycontrol.Domain.Entities;
using keycontrol.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace keycontrol.Infrastructure.EntityConfigurations;

public class TokenConfiguration : IEntityTypeConfiguration<Token>
{
    public void Configure(EntityTypeBuilder<Token> builder)
    {
        builder.HasKey(t => t.Id);
        builder.HasIndex(t => t.ExternalId);
        builder.Property(t => t.Email).IsRequired()
      .HasMaxLength(200)
      .HasConversion(
          email => email.EmailValue,
          value => Email.Create(value).Value
      );
        builder.Property(t => t.RefreshToken).IsRequired().HasMaxLength(300);
    }
}