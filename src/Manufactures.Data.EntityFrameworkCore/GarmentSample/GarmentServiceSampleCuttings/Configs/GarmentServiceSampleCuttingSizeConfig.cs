using Manufactures.Domain.GarmentSample.ServiceSampleCuttings.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.GarmentServiceSampleCuttings.Configs
{
    public class GarmentServiceSampleCuttingSizeConfig : IEntityTypeConfiguration<GarmentServiceSampleCuttingSizeReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentServiceSampleCuttingSizeReadModel> builder)
        {
            builder.ToTable("GarmentServiceSampleCuttingSizes");
            builder.HasKey(e => e.Identity);

            builder.Property(p => p.ProductCode).HasMaxLength(25);
            builder.Property(p => p.ProductName).HasMaxLength(100);
            builder.Property(p => p.SizeName).HasMaxLength(100);
            builder.Property(p => p.UomUnit).HasMaxLength(10);
            builder.Property(p => p.Color).HasMaxLength(2000);
            builder.HasOne(w => w.GarmentServiceSampleCuttingDetail)
                .WithMany(h => h.GarmentServiceSampleCuttingSizes)
                .HasForeignKey(f => f.ServiceSampleCuttingDetailId);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
