using Manufactures.Domain.GarmentLoadings.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingIns.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GermentReciptSubcon.GarmentLoadings.Configs
{
    public class GarmentSubconLoadingInItemConfig : IEntityTypeConfiguration<GarmentSubconLoadingInItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSubconLoadingInItemReadModel> builder)
        {
            builder.ToTable("GarmentSubconLoadingInItems");
            builder.HasKey(e => e.Identity);

            builder.Property(p => p.ProductCode).HasMaxLength(50);
            builder.Property(p => p.ProductName).HasMaxLength(500);
            builder.Property(p => p.Color).HasMaxLength(1000);
            builder.Property(p => p.DesignColor).HasMaxLength(2000);
            builder.Property(p => p.SizeName).HasMaxLength(50);
            builder.Property(p => p.UomUnit).HasMaxLength(50);

            builder.HasOne(w => w.GarmentLoading)
                .WithMany(h => h.Items)
                .HasForeignKey(f => f.LoadingId);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
