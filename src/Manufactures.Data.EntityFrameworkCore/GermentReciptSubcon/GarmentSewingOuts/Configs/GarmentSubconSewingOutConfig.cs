using Manufactures.Domain.GermentReciptSubcon.GarmentSewingOuts.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GermentReciptSubcon.GarmentSewingOuts.Configs
{
    public class GarmentSubconSewingOutConfig : IEntityTypeConfiguration<GarmentSubconSewingOutReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSubconSewingOutReadModel> builder)
        {
            builder.ToTable("GarmentSubconSewingOuts");
            builder.HasKey(e => e.Identity);

            builder.Property(a => a.SewingOutNo).HasMaxLength(25);
            builder.Property(a => a.ProductOwnerName).HasMaxLength(100);
            builder.Property(a => a.ProductOwnerCode).HasMaxLength(25);
            builder.Property(a => a.UnitToCode).HasMaxLength(25);
            builder.Property(a => a.UnitToName).HasMaxLength(100);
            builder.Property(a => a.RONo).HasMaxLength(25);
            builder.Property(a => a.Article).HasMaxLength(50);
            builder.Property(a => a.UnitCode).HasMaxLength(25);
            builder.Property(a => a.UnitName).HasMaxLength(100);
            builder.Property(a => a.ComodityCode).HasMaxLength(25);
            builder.Property(a => a.ComodityName).HasMaxLength(100);
            builder.Property(a => a.SewingTo).HasMaxLength(100);

            builder.HasIndex(i => i.SewingOutNo).IsUnique().HasFilter("[Deleted]=(0)");

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
