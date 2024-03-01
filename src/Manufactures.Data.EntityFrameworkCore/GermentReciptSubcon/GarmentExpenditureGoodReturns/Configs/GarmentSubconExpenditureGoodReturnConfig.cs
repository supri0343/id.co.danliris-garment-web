using Manufactures.Domain.GermentReciptSubcon.GarmentExpenditureGoodReturns.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GermentReciptSubcon.GarmentExpenditureGoodReturns.Configs
{
    public class GarmentSubconExpenditureGoodReturnConfig : IEntityTypeConfiguration<GarmentSubconExpenditureGoodReturnReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSubconExpenditureGoodReturnReadModel> builder)
        {
            builder.ToTable("GarmentSubconExpenditureGoodReturns");
            builder.HasKey(e => e.Identity);

            builder.Property(a => a.ReturNo).HasMaxLength(25);
            builder.Property(a => a.PackingOutNo).HasMaxLength(50);
            builder.Property(a => a.DONo).HasMaxLength(50);
            builder.Property(a => a.URNNo).HasMaxLength(50);
            builder.Property(a => a.BCNo).HasMaxLength(50);
            builder.Property(a => a.BCType).HasMaxLength(50);
            builder.Property(a => a.RONo).HasMaxLength(25);
            builder.Property(a => a.Article).HasMaxLength(50);
            builder.Property(a => a.UnitCode).HasMaxLength(25);
            builder.Property(a => a.UnitName).HasMaxLength(100);
            builder.Property(a => a.ComodityCode).HasMaxLength(25);
            builder.Property(a => a.ComodityName).HasMaxLength(100);
            builder.Property(a => a.Invoice).HasMaxLength(50);
            builder.Property(a => a.BuyerName).HasMaxLength(100);
            builder.Property(a => a.BuyerCode).HasMaxLength(25);
            builder.Property(a => a.ReturType).HasMaxLength(25);
            builder.Property(a => a.ReturDesc).HasMaxLength(500);

            builder.HasIndex(i => i.ReturNo).IsUnique().HasFilter("[Deleted]=(0)");

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
