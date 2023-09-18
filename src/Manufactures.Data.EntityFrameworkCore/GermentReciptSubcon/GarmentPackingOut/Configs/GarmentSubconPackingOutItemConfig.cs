using Manufactures.Domain.GarmentPackingOut.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentPackingOut.Configs
{
    public class GarmentSubconPackingOutItemConfig : IEntityTypeConfiguration<GarmentSubconPackingOutItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSubconPackingOutItemReadModel> builder)
        {
            builder.ToTable("GarmentSubconPackingOutItems");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentSubconPackingOut)
                   .WithMany(a => a.Items)
                   .HasForeignKey(a => a.PackingOutId);

            builder.Property(a => a.SizeName).HasMaxLength(100);
            builder.Property(a => a.UomUnit).HasMaxLength(10);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
