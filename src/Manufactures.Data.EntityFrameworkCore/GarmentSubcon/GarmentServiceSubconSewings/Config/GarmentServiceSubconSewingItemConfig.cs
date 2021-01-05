using System;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.GarmentServiceSubconSewings.Config
{
    public class GarmentServiceSubconSewingItemConfig : IEntityTypeConfiguration<GarmentServiceSubconSewingItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentServiceSubconSewingItemReadModel> builder)
        {
            builder.ToTable("GarmentServiceSubconSewingItems");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentServiceSubconSewingIdentity)
                   .WithMany(a => a.GarmentServiceSubconSewingItem)
                   .HasForeignKey(a => a.ServiceSubconSewingId);

            builder.Property(a => a.ProductCode)
               .HasMaxLength(25);
            builder.Property(a => a.ProductName)
               .HasMaxLength(100);
            builder.Property(a => a.DesignColor)
               .HasMaxLength(2000);

            builder.Property(a => a.SizeName)
               .HasMaxLength(100);

            builder.Property(a => a.UomUnit)
               .HasMaxLength(25);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
