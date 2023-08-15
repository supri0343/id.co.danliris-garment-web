using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.GarmentServiceSampleFabricWashes.Config
{
    public class GarmentServiceSampleFabricWashConfig : IEntityTypeConfiguration<GarmentServiceSampleFabricWashReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentServiceSampleFabricWashReadModel> builder)
        {
            builder.ToTable("GarmentServiceSampleFabricWashes");
            builder.HasKey(e => e.Identity);

            builder.Property(a => a.ServiceSampleFabricWashNo).HasMaxLength(25);

            builder.HasIndex(i => i.ServiceSampleFabricWashNo).IsUnique().HasFilter("[Deleted]=(0)");
            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
