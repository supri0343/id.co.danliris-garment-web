using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.GarmentServiceSubconCuttings.Configs
{
    public class GarmentServiceSubconCuttingItemConfig : IEntityTypeConfiguration<GarmentServiceSubconCuttingItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentServiceSubconCuttingItemReadModel> builder)
        {
            builder.ToTable("GarmentServiceSubconCuttingItems");
            builder.HasKey(e => e.Identity);

            builder.Property(p => p.ProductCode).HasMaxLength(25);
            builder.Property(p => p.ProductName).HasMaxLength(100);
            builder.Property(p => p.DesignColor).HasMaxLength(2000);
            builder.HasOne(w => w.GarmentServiceSubconCutting)
                .WithMany(h => h.GarmentServiceSubconCuttingItem)
                .HasForeignKey(f => f.ServiceSubconCuttingId);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}