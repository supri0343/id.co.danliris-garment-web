using Manufactures.Domain.GarmentSample.ServiceSampleCuttings.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.GarmentServiceSampleCuttings.Configs
{
    public class GarmentServiceSampleCuttingConfig : IEntityTypeConfiguration<GarmentServiceSampleCuttingReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentServiceSampleCuttingReadModel> builder)
        {
            builder.ToTable("GarmentServiceSampleCuttings");
            builder.HasKey(e => e.Identity);

            builder.Property(p => p.SampleNo).HasMaxLength(25);
            builder.Property(p => p.UnitCode).HasMaxLength(25);
            builder.Property(p => p.UnitName).HasMaxLength(100);
            builder.HasIndex(i => i.SampleNo).IsUnique().HasFilter("[Deleted]=(0)");

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}