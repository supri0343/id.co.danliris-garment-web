using Manufactures.Domain.GarmentSample.ServiceSampleSewings.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.GarmentServiceSampleSewings.Config
{
    public class GarmentServiceSampleSewingDetailConfig : IEntityTypeConfiguration<GarmentServiceSampleSewingDetailReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentServiceSampleSewingDetailReadModel> builder)
        {
            builder.ToTable("GarmentServiceSampleSewingDetails");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentServiceSampleSewingItemIdentity)
                   .WithMany(a => a.GarmentServiceSampleSewingDetail)
                   .HasForeignKey(a => a.ServiceSampleSewingItemId);

            builder.Property(a => a.ProductCode)
               .HasMaxLength(25);
            builder.Property(a => a.ProductName)
               .HasMaxLength(100);
            builder.Property(a => a.DesignColor)
               .HasMaxLength(2000);

            builder.Property(a => a.UomUnit)
               .HasMaxLength(25);

            builder.Property(a => a.Remark)
               .HasMaxLength(2000);

            builder.Property(a => a.UnitCode).HasMaxLength(25);
            builder.Property(a => a.UnitName).HasMaxLength(100);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
