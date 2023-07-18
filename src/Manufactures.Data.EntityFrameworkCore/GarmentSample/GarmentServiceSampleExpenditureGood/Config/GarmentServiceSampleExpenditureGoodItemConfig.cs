using System;
using Manufactures.Domain.GarmentSample.ServiceSampleExpenditureGood.ReadModels;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.ServiceSampleExpenditureGood.Config
{
    public class GarmentServiceSampleExpenditureGoodItemConfig : IEntityTypeConfiguration<GarmentServiceSampleExpenditureGoodItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentServiceSampleExpenditureGoodItemReadModel> builder)
        {
            builder.ToTable("GarmentServiceSampleExpenditureGoodItems");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentServiceSampleExpenditureGoodIdentity)
                   .WithMany(a => a.GarmentServiceSampleExpenditureGoodItem)
                   .HasForeignKey(a => a.ServiceSampleExpenditureGoodId);

            builder.Property(a => a.RONo).HasMaxLength(25);
            builder.Property(a => a.Article).HasMaxLength(50);
            builder.Property(a => a.ComodityCode).HasMaxLength(25);
            builder.Property(a => a.ComodityName).HasMaxLength(100);
            //builder.Property(a => a.BuyerName).HasMaxLength(100);
            //builder.Property(a => a.BuyerCode).HasMaxLength(25);
            builder.Property(a => a.UomUnit).HasMaxLength(25);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
