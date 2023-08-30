using Manufactures.Domain.GermentReciptSubcon.GarmentPackingIns.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GermentReciptSubcon.GarmentPackingIns.Configs
{
    public class GarmentSubconPackingInConfig : IEntityTypeConfiguration<GarmentSubconPackingInReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSubconPackingInReadModel> builder)
        {
            builder.ToTable("GarmentSubconPackingIns");
            builder.HasKey(e => e.Identity);

            builder.Property(a => a.PackingInNo).HasMaxLength(25);
            builder.Property(a => a.UnitFromCode).HasMaxLength(25);
            builder.Property(a => a.UnitFromName).HasMaxLength(100);
            builder.Property(a => a.UnitCode).HasMaxLength(25);
            builder.Property(a => a.UnitName).HasMaxLength(100);
            builder.Property(a => a.RONo).HasMaxLength(25);
            builder.Property(a => a.Article).HasMaxLength(2000);
            builder.Property(a => a.ComodityCode).HasMaxLength(25);
            builder.Property(a => a.ComodityName).HasMaxLength(100);

            builder.HasIndex(i => i.PackingInNo).IsUnique().HasFilter("[Deleted]=(0)");

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
