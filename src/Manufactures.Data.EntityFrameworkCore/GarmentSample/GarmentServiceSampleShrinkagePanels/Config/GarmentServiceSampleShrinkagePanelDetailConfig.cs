using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.GarmentServiceSampleShrinkagePanels.Config
{
    public class GarmentServiceSampleShrinkagePanelDetailConfig : IEntityTypeConfiguration<GarmentServiceSampleShrinkagePanelDetailReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentServiceSampleShrinkagePanelDetailReadModel> builder)
        {
            builder.ToTable("GarmentServiceSampleShrinkagePanelDetails");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentServiceSampleShrinkagePanelItemIdentity)
                   .WithMany(a => a.GarmentServiceSampleShrinkagePanelDetail)
                   .HasForeignKey(a => a.ServiceSampleShrinkagePanelItemId);

            builder.Property(a => a.ProductCode).HasMaxLength(25);
            builder.Property(a => a.ProductName).HasMaxLength(100);
            builder.Property(a => a.ProductRemark).HasMaxLength(1000);
            builder.Property(a => a.DesignColor).HasMaxLength(100);
            builder.Property(a => a.UomUnit).HasMaxLength(100);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
