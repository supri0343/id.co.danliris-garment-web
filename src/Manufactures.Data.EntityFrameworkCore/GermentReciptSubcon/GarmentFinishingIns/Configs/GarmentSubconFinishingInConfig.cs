using Manufactures.Domain.GarmentFinishingIns.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingIns.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GermentReciptSubcon.GarmentFinishingIns.Configs
{
    public class GarmentSubconFinishingInConfig : IEntityTypeConfiguration<GarmentSubconFinishingInReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSubconFinishingInReadModel> builder)
        {
            builder.ToTable("GarmentSubconFinishingIns");
            builder.HasKey(e => e.Identity);

            builder.Property(a => a.FinishingInNo).HasMaxLength(25);
            builder.Property(a => a.UnitFromCode).HasMaxLength(25);
            builder.Property(a => a.UnitFromName).HasMaxLength(100);
            builder.Property(a => a.UnitCode).HasMaxLength(25);
            builder.Property(a => a.UnitName).HasMaxLength(100);
            builder.Property(a => a.RONo).HasMaxLength(25);
            builder.Property(a => a.Article).HasMaxLength(50);
            builder.Property(a => a.ComodityCode).HasMaxLength(25);
            builder.Property(a => a.ComodityName).HasMaxLength(100);

            builder.Property(a => a.DONo).HasMaxLength(100);
            builder.Property(a => a.SubconType).HasMaxLength(100);

            builder.HasIndex(i => i.FinishingInNo).IsUnique().HasFilter("[Deleted]=(0)");

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
