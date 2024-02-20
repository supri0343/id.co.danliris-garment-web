using Manufactures.Domain.GarmentSubcon.CustomsOuts.ReadModels;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.CustomsOuts.Cofigs
{
    public class GarmentSubconCustomsOutDetailConfig : IEntityTypeConfiguration<GarmentSubconCustomsOutDetailReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSubconCustomsOutDetailReadModel> builder)
        {
            builder.ToTable("GarmentSubconCustomsOutDetails");
            builder.HasKey(e => e.Identity);

            builder.Property(p => p.ProductCode).HasMaxLength(25);
            builder.Property(p => p.ProductName).HasMaxLength(100);

            builder.Property(p => p.ProductRemark).HasMaxLength(2000);
            builder.Property(p => p.UomUnit).HasMaxLength(50);

            builder.HasOne(w => w.GarmentSubconCustomsOutItem)
                .WithMany(h => h.GarmentSubconCustomsOutDetail)
                .HasForeignKey(f => f.SubconCustomsOutItemId);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
