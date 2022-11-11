using Manufactures.Domain.GarmentSubcon.SubconReprocess.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.SubconReprocess.Config
{
    public class GarmentSubconReprocessDetailConfig : IEntityTypeConfiguration<GarmentSubconReprocessDetailReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSubconReprocessDetailReadModel> builder)
        {
            builder.ToTable("GarmentSubconReprocessDetails");
            builder.HasKey(e => e.Identity);

            builder.Property(p => p.DesignColor).HasMaxLength(2000);
            builder.Property(p => p.Color).HasMaxLength(2000);
            builder.Property(p => p.UomUnit).HasMaxLength(20);
            builder.Property(p => p.UnitCode).HasMaxLength(20);
            builder.Property(p => p.UnitName).HasMaxLength(25);
            builder.HasOne(w => w.GarmentSubconReprocessItem)
                .WithMany(h => h.GarmentSubconReprocessDetail)
                .HasForeignKey(f => f.ReprocessItemId);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
