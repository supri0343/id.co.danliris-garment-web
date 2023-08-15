using System;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.GarmentServiceSampleSewings.Config
{
    public class GarmentServiceSampleSewingConfig : IEntityTypeConfiguration<GarmentServiceSampleSewingReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentServiceSampleSewingReadModel> builder)
        {
            builder.ToTable("GarmentServiceSampleSewings");
            builder.HasKey(e => e.Identity);

            builder.Property(a => a.ServiceSampleSewingNo).HasMaxLength(25);

            builder.HasIndex(i => i.ServiceSampleSewingNo).IsUnique().HasFilter("[Deleted]=(0)");
            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
