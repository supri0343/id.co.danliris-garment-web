using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.GarmentServiceSampleFabricWashes.Config
{
    public class GarmentServiceSampleFabricWashItemConfig : IEntityTypeConfiguration<GarmentServiceSampleFabricWashItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentServiceSampleFabricWashItemReadModel> builder)
        {
            builder.ToTable("GarmentServiceSampleFabricWashItems");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentServiceSampleFabricWashIdentity)
                   .WithMany(a => a.GarmentServiceSampleFabricWashItem)
                   .HasForeignKey(a => a.ServiceSampleFabricWashId);


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
