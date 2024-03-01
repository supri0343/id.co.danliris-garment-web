using Manufactures.Domain.GermentReciptSubcon.GarmentExpenditureGoodReturns.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GermentReciptSubcon.GarmentExpenditureGoodReturns.Configs
{
    public class GarmentSubconExpenditureGoodReturnItemConfig : IEntityTypeConfiguration<GarmentSubconExpenditureGoodReturnItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSubconExpenditureGoodReturnItemReadModel> builder)
        {
            builder.ToTable("GarmentSubconExpenditureGoodReturnItems");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentSubconExpenditureGoodReturn)
                   .WithMany(a => a.Items)
                   .HasForeignKey(a => a.ReturId);

            builder.Property(a => a.SizeName).HasMaxLength(100);
            builder.Property(a => a.UomUnit).HasMaxLength(10);
            builder.Property(a => a.Description).HasMaxLength(2000);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
