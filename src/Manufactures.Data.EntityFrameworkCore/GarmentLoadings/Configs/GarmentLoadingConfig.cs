using Manufactures.Domain.GarmentLoadings.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentLoadings.Configs
{
    public class GarmentLoadingConfig : IEntityTypeConfiguration<GarmentLoadingReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentLoadingReadModel> builder)
        {
            builder.ToTable("GarmentLoadings");
            builder.HasKey(e => e.Identity);

            builder.Property(p => p.LoadingNo).HasMaxLength(25);
            builder.Property(p => p.SewingDONo).HasMaxLength(25);
            builder.Property(p => p.RONo).HasMaxLength(25);
            builder.Property(p => p.Article).HasMaxLength(50);
            builder.Property(p => p.UnitCode).HasMaxLength(25);
            builder.Property(p => p.UnitName).HasMaxLength(100);
            builder.Property(p => p.Comodity).HasMaxLength(500);
            builder.Property(p => p.UnitFromCode).HasMaxLength(25);
            builder.Property(p => p.UnitFromName).HasMaxLength(100);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
