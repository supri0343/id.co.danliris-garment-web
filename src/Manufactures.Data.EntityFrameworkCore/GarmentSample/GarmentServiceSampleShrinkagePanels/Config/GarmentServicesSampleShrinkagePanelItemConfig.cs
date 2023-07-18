using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.GarmentServiceSampleShrinkagePanels.Config
{
    public class GarmentServiceSampleShrinkagePanelItemConfig : IEntityTypeConfiguration<GarmentServiceSampleShrinkagePanelItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentServiceSampleShrinkagePanelItemReadModel> builder)
        {
            builder.ToTable("GarmentServiceSampleShrinkagePanelItems");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentServiceSampleShrinkagePanelIdentity)
                   .WithMany(a => a.GarmentServiceSampleShrinkagePanelItem)
                   .HasForeignKey(a => a.ServiceSampleShrinkagePanelId);


            builder.Property(a => a.UnitExpenditureNo).HasMaxLength(25);
            builder.Property(a => a.UnitSenderCode).HasMaxLength(25);
            builder.Property(a => a.UnitSenderName).HasMaxLength(100);
            builder.Property(a => a.UnitRequestCode).HasMaxLength(25);
            builder.Property(a => a.UnitRequestName).HasMaxLength(100);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
