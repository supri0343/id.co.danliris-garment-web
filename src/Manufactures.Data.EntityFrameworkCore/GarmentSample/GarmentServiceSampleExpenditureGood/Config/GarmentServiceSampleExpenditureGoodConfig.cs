using System;
using Manufactures.Domain.GarmentSample.ServiceSampleExpenditureGood.ReadModels;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.ServiceSampleExpenditureGood.Config
{
    public class GarmentServiceSampleExpenditureGoodConfig : IEntityTypeConfiguration<GarmentServiceSampleExpenditureGoodReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentServiceSampleExpenditureGoodReadModel> builder)
        {
            builder.ToTable("GarmentServiceSampleExpenditureGoods");
            builder.HasKey(e => e.Identity);

            builder.Property(a => a.ServiceSampleExpenditureGoodNo).HasMaxLength(25);

            builder.HasIndex(i => i.ServiceSampleExpenditureGoodNo).IsUnique().HasFilter("[Deleted]=(0)");
            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
