using System;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.GarmentServiceSampleSewings.Config
{
    public class GarmentServiceSampleSewingItemConfig : IEntityTypeConfiguration<GarmentServiceSampleSewingItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentServiceSampleSewingItemReadModel> builder)
        {
            builder.ToTable("GarmentServiceSampleSewingItems");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentServiceSampleSewingIdentity)
                   .WithMany(a => a.GarmentServiceSampleSewingItem)
                   .HasForeignKey(a => a.ServiceSampleSewingId);


            builder.Property(a => a.RONo).HasMaxLength(25);
            builder.Property(a => a.Article).HasMaxLength(50);
            builder.Property(a => a.ComodityCode).HasMaxLength(25);
            builder.Property(a => a.ComodityName).HasMaxLength(100);
            builder.Property(a => a.BuyerName).HasMaxLength(100);
            builder.Property(a => a.BuyerCode).HasMaxLength(25);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
