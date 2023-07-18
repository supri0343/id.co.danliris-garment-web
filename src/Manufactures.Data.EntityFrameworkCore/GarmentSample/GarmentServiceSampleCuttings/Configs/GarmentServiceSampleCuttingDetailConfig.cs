using Manufactures.Domain.GarmentSample.ServiceSampleCuttings.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.GarmentServiceSampleCuttings.Configs
{
    public class GarmentServiceSampleCuttingDetailConfig : IEntityTypeConfiguration<GarmentServiceSampleCuttingDetailReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentServiceSampleCuttingDetailReadModel> builder)
        {
            builder.ToTable("GarmentServiceSampleCuttingDetails");
            builder.HasKey(e => e.Identity);

            builder.Property(p => p.DesignColor).HasMaxLength(2000);
            builder.HasOne(w => w.GarmentServiceSampleCuttingItem)
                .WithMany(h => h.GarmentServiceSampleCuttingDetail)
                .HasForeignKey(f => f.ServiceSampleCuttingItemId);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}