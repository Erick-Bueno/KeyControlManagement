using keycontrol.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace keycontrol.Infrastructure.EntityConfigurations;

public class KeyConfiguration : IEntityTypeConfiguration<Key>
{
    public void Configure(EntityTypeBuilder<Key> builder)
    {
        builder.HasKey(k => k.Id);
        builder.HasIndex(k => k.ExternalId);
        builder.HasOne(k => k.Room)
            .WithMany(r => r.Keys)
            .HasForeignKey(k => k.IdRoom);
    }
}