using Manufactures.Domain.GarmentFinishingOuts.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingOuts.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GermentReciptSubcon.GarmentFinishingOuts.Configs
{
    public class GarmentSubconFinishingOutDetailConfig : IEntityTypeConfiguration<GarmentReceiptSubconFinishingOutDetailReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentReceiptSubconFinishingOutDetailReadModel> builder)
        {
            builder.ToTable("GarmentSubconFinishingOutDetails");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentFinishingOutItemIdentity)
               .WithMany(a => a.GarmentFinishingOutDetail)
               .HasForeignKey(a => a.FinishingOutItemId);

            builder.Property(a => a.SizeName)
               .HasMaxLength(100);
            builder.Property(a => a.UomUnit)
               .HasMaxLength(25);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
