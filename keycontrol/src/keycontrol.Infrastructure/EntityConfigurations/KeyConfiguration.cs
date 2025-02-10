using keycontrol.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace keycontrol.Infrastructure.EntityConfigurations;

public class KeyConfiguration : IEntityTypeConfiguration<KeyRoom>
{
    public void Configure(EntityTypeBuilder<KeyRoom> builder)
    {
        builder.HasKey(k => k.Id);
        builder.HasIndex(k => k.ExternalId);
        builder.Property(k => k.Status).IsRequired();
        builder.HasOne(k => k.Room)
            .WithMany(r => r.Keys)
            .HasForeignKey(k => k.IdRoom);
        builder.Property(k => k.Description).HasMaxLength(500);
    }
}