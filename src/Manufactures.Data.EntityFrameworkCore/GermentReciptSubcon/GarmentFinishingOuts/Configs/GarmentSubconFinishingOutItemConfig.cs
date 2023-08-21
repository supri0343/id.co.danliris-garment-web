using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingOuts.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GermentReciptSubcon.GarmentFinishingOuts.Configs
{
    public class GarmentSubconFinishingOutItemConfig : IEntityTypeConfiguration<GarmentReceiptSubconFinishingOutItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentReceiptSubconFinishingOutItemReadModel> builder)
        {
            builder.ToTable("GarmentSubconFinishingOutItems");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentFinishingOutIdentity)
                   .WithMany(a => a.GarmentFinishingOutItem)
                   .HasForeignKey(a => a.FinishingOutId);

            builder.Property(a => a.ProductCode)
               .HasMaxLength(25);
            builder.Property(a => a.ProductName)
               .HasMaxLength(100);
            builder.Property(a => a.DesignColor)
               .HasMaxLength(2000);

            builder.Property(a => a.SizeName)
               .HasMaxLength(100);

            builder.Property(a => a.UomUnit)
               .HasMaxLength(25);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
