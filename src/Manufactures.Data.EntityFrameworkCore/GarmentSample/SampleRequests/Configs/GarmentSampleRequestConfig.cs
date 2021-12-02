using Manufactures.Domain.GarmentSample.SampleRequests.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleRequests.Configs
{
    public class GarmentSampleRequestConfig : IEntityTypeConfiguration<GarmentSampleRequestReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSampleRequestReadModel> builder)
        {
            builder.ToTable("GarmentSampleRequests");
            builder.HasKey(e => e.Identity);

            builder.Property(p => p.Attached).HasMaxLength(255);
            builder.Property(p => p.SampleCategory).HasMaxLength(50);
            builder.Property(p => p.ComodityCode).HasMaxLength(25);
            builder.Property(p => p.ComodityName).HasMaxLength(255);
            builder.Property(p => p.Remark).HasMaxLength(4000);
            builder.Property(a => a.BuyerName).HasMaxLength(255);
            builder.Property(p => p.BuyerCode).HasMaxLength(25);
            builder.Property(p => p.Packing).HasMaxLength(255);
            builder.Property(p => p.SampleRequestNo).HasMaxLength(30);
            builder.Property(p => p.RONoCC).HasMaxLength(15);
            builder.Property(p => p.RONoSample).HasMaxLength(15);
            builder.Property(p => p.POBuyer).HasMaxLength(255);
            builder.Property(p => p.Remark).HasMaxLength(4000);
            builder.Property(p => p.SampleType).HasMaxLength(255);
            builder.Property(p => p.ReceivedBy).HasMaxLength(255);
            builder.HasIndex(i => i.SampleRequestNo).IsUnique().HasFilter("[Deleted]=(0)");

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
