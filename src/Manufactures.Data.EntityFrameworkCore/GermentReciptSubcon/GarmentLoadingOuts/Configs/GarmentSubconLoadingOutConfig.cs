using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingOuts.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GermentReciptSubcon.GarmentLoadingOuts.Configs
{
    public class GarmentSubconLoadingOutConfig : IEntityTypeConfiguration<GarmentSubconLoadingOutReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSubconLoadingOutReadModel> builder)
        {
            builder.ToTable("GarmentSubconLoadingOuts");
            builder.HasKey(e => e.Identity);

            builder.Property(p => p.LoadingOutNo).HasMaxLength(25);
            builder.Property(p => p.LoadingInNo).HasMaxLength(25);
            builder.Property(p => p.RONo).HasMaxLength(25);
            builder.Property(p => p.Article).HasMaxLength(50);
            builder.Property(p => p.UnitCode).HasMaxLength(25);
            builder.Property(p => p.UnitName).HasMaxLength(100);
            builder.Property(p => p.ComodityName).HasMaxLength(500);
            builder.Property(p => p.ComodityCode).HasMaxLength(100);
            builder.Property(p => p.UnitFromCode).HasMaxLength(25);
            builder.Property(p => p.UnitFromName).HasMaxLength(100);

            builder.HasIndex(i => i.LoadingOutNo).IsUnique().HasFilter("[Deleted]=(0)");

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
