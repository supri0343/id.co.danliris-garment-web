using System;
using Manufactures.Domain.GarmentSubcon.ServiceSubconExpenditureGood.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.ServiceSubconExpenditureGood.Config
{
    public class GarmentServiceSubconExpenditureGoodItemConfig : IEntityTypeConfiguration<GarmentServiceSubconExpenditureGoodItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentServiceSubconExpenditureGoodItemReadModel> builder)
        {
            builder.ToTable("GarmentServiceSubconExpenditureGoodItems");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentServiceSubconExpenditureGoodIdentity)
                   .WithMany(a => a.GarmentServiceSubconExpenditureGoodItem)
                   .HasForeignKey(a => a.ServiceSubconExpenditureGoodId);

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
