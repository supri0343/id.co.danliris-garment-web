using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.SubconDeliveryLetterOuts.Cofigs
{
    public class GarmentSubconDeliveryLetterOutDetailConfig : IEntityTypeConfiguration<GarmentSubconDeliveryLetterOutDetailReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSubconDeliveryLetterOutDetailReadModel> builder)
        {
            builder.ToTable("GarmentSubconDeliveryLetterOutDetails");
            builder.HasKey(e => e.Identity);

            builder.Property(p => p.ProductCode).HasMaxLength(25);
            builder.Property(p => p.ProductName).HasMaxLength(100);
            builder.Property(p => p.DesignColor).HasMaxLength(2000);
            builder.Property(p => p.ProductRemark).HasMaxLength(2000);
            builder.Property(p => p.UomUnit).HasMaxLength(50);
            builder.Property(p => p.UomOutUnit).HasMaxLength(50);
            builder.Property(p => p.FabricType).HasMaxLength(255);

            builder.Property(p => p.UENNo).HasMaxLength(100);
            builder.HasOne(w => w.GarmentSubconDeliveryLetterOutItem)
                .WithMany(h => h.GarmentSubconDeliveryLetterOutDetail)
                .HasForeignKey(f => f.SubconDeliveryLetterOutItemId);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
