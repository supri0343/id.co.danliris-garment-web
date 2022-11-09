using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.ReadModels;
using Manufactures.Domain.GarmentSubcon.SubconReprocess.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.SubconReprocess.Config
{
    public class GarmentSubconReprocessConfig : IEntityTypeConfiguration<GarmentSubconReprocessReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSubconReprocessReadModel> builder)
        {
            builder.ToTable("GarmentSubconReprocesses");
            builder.HasKey(e => e.Identity);

            builder.Property(a => a.ReprocessType).HasMaxLength(50);
            builder.Property(a => a.ReprocessNo).HasMaxLength(20);

            builder.HasIndex(i => i.ReprocessNo).IsUnique().HasFilter("[Deleted]=(0)");
            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
