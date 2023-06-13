using System;
using Manufactures.Domain.GarmentSubcon.ServiceSubconExpenditureGood.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.ServiceSubconExpenditureGood.Config
{
    public class GarmentServiceSubconExpenditureGoodConfig : IEntityTypeConfiguration<GarmentServiceSubconExpenditureGoodReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentServiceSubconExpenditureGoodReadModel> builder)
        {
            builder.ToTable("GarmentServiceSubconExpenditureGoods");
            builder.HasKey(e => e.Identity);

            builder.Property(a => a.ServiceSubconExpenditureGoodNo).HasMaxLength(25);

            builder.HasIndex(i => i.ServiceSubconExpenditureGoodNo).IsUnique().HasFilter("[Deleted]=(0)");
            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
