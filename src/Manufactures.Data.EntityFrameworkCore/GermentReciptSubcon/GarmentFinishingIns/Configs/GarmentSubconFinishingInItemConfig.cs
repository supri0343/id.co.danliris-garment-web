using Manufactures.Domain.GarmentFinishingIns.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingIns.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GermentReciptSubcon.GarmentFinishingIns.Configs
{
    public class GarmentSubconFinishingInItemConfig : IEntityTypeConfiguration<GarmentSubconFinishingInItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSubconFinishingInItemReadModel> builder)
        {
            builder.ToTable("GarmentSubconFinishingInItems");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentFinishingIn)
                   .WithMany(a => a.Items)
                   .HasForeignKey(a => a.FinishingInId);

            builder.Property(a => a.ProductCode).HasMaxLength(25);
            builder.Property(a => a.ProductName).HasMaxLength(100);
            builder.Property(a => a.DesignColor).HasMaxLength(2000);
            builder.Property(a => a.SizeName).HasMaxLength(100);
            builder.Property(a => a.UomUnit).HasMaxLength(10);
            builder.Property(a => a.Color).HasMaxLength(1000);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
