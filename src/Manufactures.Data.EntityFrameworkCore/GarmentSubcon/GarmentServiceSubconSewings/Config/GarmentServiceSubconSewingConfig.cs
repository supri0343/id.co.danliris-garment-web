using System;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.GarmentServiceSubconSewings.Config
{
    public class GarmentServiceSubconSewingConfig : IEntityTypeConfiguration<GarmentServiceSubconSewingReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentServiceSubconSewingReadModel> builder)
        {
            builder.ToTable("GarmentServiceSubconSewings");
            builder.HasKey(e => e.Identity);

            builder.Property(a => a.ServiceSubconSewingNo).HasMaxLength(25);
            builder.Property(a => a.BuyerName).HasMaxLength(100);
            builder.Property(a => a.BuyerCode).HasMaxLength(25);
            builder.Property(a => a.RONo).HasMaxLength(25);
            builder.Property(a => a.Article).HasMaxLength(50);
            builder.Property(a => a.UnitCode).HasMaxLength(25);
            builder.Property(a => a.UnitName).HasMaxLength(100);
            builder.Property(a => a.ComodityCode).HasMaxLength(25);
            builder.Property(a => a.ComodityName).HasMaxLength(100);

            builder.HasIndex(i => i.ServiceSubconSewingNo).IsUnique().HasFilter("[Deleted]=(0)");
            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
