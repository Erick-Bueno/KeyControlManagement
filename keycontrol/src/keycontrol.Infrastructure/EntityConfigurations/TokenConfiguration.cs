using keycontrol.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace keycontrol.Infrastructure.EntityConfigurations;

public class TokenConfiguration : IEntityTypeConfiguration<Token>
{
    public void Configure(EntityTypeBuilder<Token> builder)
    {
        builder.HasKey(t => t.Id);
        builder.HasIndex(t => t.ExternalId);
        builder.Property(t => t.Email).IsRequired().HasMaxLength(200);
        builder.Property(t => t.RefreshToken).IsRequired().HasMaxLength(300);
    }
}