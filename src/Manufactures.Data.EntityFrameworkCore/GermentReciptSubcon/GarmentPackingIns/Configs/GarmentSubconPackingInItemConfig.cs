using Manufactures.Domain.GermentReciptSubcon.GarmentPackingIns.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GermentReciptSubcon.GarmentPackingIns.Configs
{
    public class GarmentSubconPackingInItemConfig : IEntityTypeConfiguration<GarmentSubconPackingInItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSubconPackingInItemReadModel> builder)
        {
            builder.ToTable("GarmentSubconPackingInItems");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentSubconPackingInIdentity)
                   .WithMany(a => a.GarmentSubconPackingInItem)
                   .HasForeignKey(a => a.PackingInId);

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
