using Manufactures.Domain.GermentReciptSubcon.GarmentSewingOuts.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GermentReciptSubcon.GarmentSewingOuts.Configs
{
    public class GarmentSubconSewingOutDetailConfig : IEntityTypeConfiguration<GarmentSubconSewingOutDetailReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSubconSewingOutDetailReadModel> builder)
        {
            builder.ToTable("GarmentSubconSewingOutDetails");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentSewingOutItemIdentity)
               .WithMany(a => a.GarmentSewingOutDetail)
               .HasForeignKey(a => a.SewingOutItemId);

            builder.Property(a => a.SizeName)
               .HasMaxLength(100);
            builder.Property(a => a.UomUnit)
               .HasMaxLength(25);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
