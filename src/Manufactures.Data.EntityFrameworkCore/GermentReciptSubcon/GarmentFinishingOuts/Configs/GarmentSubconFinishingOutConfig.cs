using Manufactures.Domain.GarmentFinishingOuts.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingOuts.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GermentReciptSubcon.GarmentFinishingOuts.Configs
{
    public class GarmentSubconFinishingOutConfig : IEntityTypeConfiguration<GarmentReceiptSubconFinishingOutReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentReceiptSubconFinishingOutReadModel> builder)
        {
            builder.ToTable("GarmentSubconFinishingOuts");
            builder.HasKey(e => e.Identity);

            builder.Property(a => a.FinishingOutNo).HasMaxLength(25);
            builder.Property(a => a.UnitToCode).HasMaxLength(25);
            builder.Property(a => a.UnitToName).HasMaxLength(100);
            builder.Property(a => a.RONo).HasMaxLength(25);
            builder.Property(a => a.Article).HasMaxLength(50);
            builder.Property(a => a.UnitCode).HasMaxLength(25);
            builder.Property(a => a.UnitName).HasMaxLength(100);
            builder.Property(a => a.ComodityCode).HasMaxLength(25);
            builder.Property(a => a.ComodityName).HasMaxLength(100);
            builder.Property(a => a.FinishingTo).HasMaxLength(100);

            builder.HasIndex(i => i.FinishingOutNo).IsUnique().HasFilter("[Deleted]=(0)");

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
