using keycontrol.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace keycontrol.Infrastructure.EntityConfigurations;

public class ReportConfiguration : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        builder.HasKey(r => r.Id);
        builder.HasIndex(r => r.ExternalId);
        builder.Property(r => r.Status).IsRequired();
        builder.Property(r => r.WithdrawalDate).IsRequired();
        builder.HasOne(r => r.User)
            .WithMany(u => u.Reports)
            .HasForeignKey(r => r.IdUser);
        builder.HasOne(r => r.Key)
            .WithMany(k => k.Reports)
            .HasForeignKey(r => r.IdKey);
    }
}