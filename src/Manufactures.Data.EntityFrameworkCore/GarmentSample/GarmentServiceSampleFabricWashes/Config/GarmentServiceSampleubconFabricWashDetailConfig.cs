using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.GarmentServiceSampleFabricWashes.Config
{
    public class GarmentServiceSampleFabricWashDetailConfig : IEntityTypeConfiguration<GarmentServiceSampleFabricWashDetailReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentServiceSampleFabricWashDetailReadModel> builder)
        {
            builder.ToTable("GarmentServiceSampleFabricWashDetails");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentServiceSampleFabricWashItemIdentity)
                   .WithMany(a => a.GarmentServiceSampleFabricWashDetail)
                   .HasForeignKey(a => a.ServiceSampleFabricWashItemId);

            builder.Property(a => a.ProductCode).HasMaxLength(25);
            builder.Property(a => a.ProductName).HasMaxLength(100);
            builder.Property(a => a.ProductRemark).HasMaxLength(1000);
            builder.Property(a => a.DesignColor).HasMaxLength(100);
            builder.Property(a => a.UomUnit).HasMaxLength(100);
            builder.Property(a => a.Quantity).HasMaxLength(100);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
