using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.GarmentServiceSampleShrinkagePanels.Config
{
    public class GarmentServiceSampleShrinkagePanelConfig : IEntityTypeConfiguration<GarmentServiceSampleShrinkagePanelReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentServiceSampleShrinkagePanelReadModel> builder)
        {
            builder.ToTable("GarmentServiceSampleShrinkagePanels");
            builder.HasKey(e => e.Identity);

            builder.Property(a => a.ServiceSampleShrinkagePanelNo).HasMaxLength(25);
            builder.Property(a => a.Remark).HasMaxLength(1000);

            builder.HasIndex(i => i.ServiceSampleShrinkagePanelNo).IsUnique().HasFilter("[Deleted]=(0)");
            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
