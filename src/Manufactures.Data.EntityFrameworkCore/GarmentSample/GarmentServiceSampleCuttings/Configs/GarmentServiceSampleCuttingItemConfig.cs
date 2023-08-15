using Manufactures.Domain.GarmentSample.ServiceSampleCuttings.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.GarmentServiceSampleCuttings.Configs
{
    public class GarmentServiceSampleCuttingItemConfig : IEntityTypeConfiguration<GarmentServiceSampleCuttingItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentServiceSampleCuttingItemReadModel> builder)
        {
            builder.ToTable("GarmentServiceSampleCuttingItems");
            builder.HasKey(e => e.Identity);

            builder.Property(p => p.RONo).HasMaxLength(25);
            builder.Property(p => p.Article).HasMaxLength(50);
            builder.Property(p => p.ComodityName).HasMaxLength(500);
            builder.Property(p => p.ComodityCode).HasMaxLength(255);
            builder.HasOne(w => w.GarmentServiceSampleCutting)
                .WithMany(h => h.GarmentServiceSampleCuttingItem)
                .HasForeignKey(f => f.ServiceSampleCuttingId);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}