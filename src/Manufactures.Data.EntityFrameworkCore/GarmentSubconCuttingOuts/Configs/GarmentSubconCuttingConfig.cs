using Manufactures.Domain.GarmentSubconCuttingOuts.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubconCuttingOuts.Configs
{
    public class GarmentSubconCuttingConfig : IEntityTypeConfiguration<GarmentSubconCuttingReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSubconCuttingReadModel> builder)
        {
            builder.ToTable("GarmentSubconCuttings");
            builder.HasKey(e => e.Identity);

            builder.Property(a => a.SizeName)
               .HasMaxLength(100);
            builder.Property(a => a.RONo)
               .HasMaxLength(50);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
