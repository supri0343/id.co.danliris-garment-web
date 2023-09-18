using Manufactures.Domain.GarmentPackingOut.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentPackingOut.Configs
{
    public class GarmentSubconPackingOutConfig : IEntityTypeConfiguration<GarmentSubconPackingOutReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSubconPackingOutReadModel> builder)
        {
            builder.ToTable("GarmentSubconPackingOuts");
            builder.HasKey(e => e.Identity);

            builder.Property(a => a.PackingOutNo).HasMaxLength(25);
            builder.Property(a => a.RONo).HasMaxLength(25);
            builder.Property(a => a.Article).HasMaxLength(50);
            builder.Property(a => a.UnitCode).HasMaxLength(25);
            builder.Property(a => a.UnitName).HasMaxLength(100);
            builder.Property(a => a.ComodityCode).HasMaxLength(25);
            builder.Property(a => a.ComodityName).HasMaxLength(100);
            builder.Property(a => a.Invoice).HasMaxLength(50);
            builder.Property(a => a.ProductOwnerName).HasMaxLength(100);
            builder.Property(a => a.ProductOwnerCode).HasMaxLength(25);
            builder.Property(a => a.PackingOutType).HasMaxLength(25);

            builder.HasIndex(i => i.PackingOutNo).IsUnique().HasFilter("[Deleted]=(0)");

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
