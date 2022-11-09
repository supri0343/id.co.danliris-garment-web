using Manufactures.Domain.GarmentSubcon.SubconReprocess.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.SubconReprocess.Config
{
    public class GarmentSubconReprocessItemConfig : IEntityTypeConfiguration<GarmentSubconReprocessItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSubconReprocessItemReadModel> builder)
        {
            builder.ToTable("GarmentSubconReprocessItems");
            builder.HasKey(e => e.Identity);

            builder.Property(p => p.RONo).HasMaxLength(20);
            builder.Property(p => p.ServiceSubconCuttingNo).HasMaxLength(25);
            builder.Property(p => p.ServiceSubconSewingNo).HasMaxLength(25);
            builder.Property(p => p.Article).HasMaxLength(50);
            builder.Property(p => p.ComodityName).HasMaxLength(500);
            builder.Property(p => p.ComodityCode).HasMaxLength(255);
            builder.Property(a => a.BuyerName).HasMaxLength(500);
            builder.Property(a => a.BuyerCode).HasMaxLength(25);
            builder.HasOne(w => w.GarmentSubconReprocess)
                .WithMany(h => h.GarmentSubconReprocessItem)
                .HasForeignKey(f => f.ReprocessId);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}