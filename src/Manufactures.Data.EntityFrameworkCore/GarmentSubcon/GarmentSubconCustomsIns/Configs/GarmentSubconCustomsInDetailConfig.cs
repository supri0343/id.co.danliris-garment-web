using Manufactures.Domain.GarmentSubcon.SubconCustomsIns.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.GarmentSubconCustomsIns.Configs
{
    public class GarmentSubconCustomsInDetailConfig : IEntityTypeConfiguration<GarmentSubconCustomsInDetailReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSubconCustomsInDetailReadModel> builder)
        {
            builder.ToTable("GarmentServiceSubconCustomsInDetails");
            builder.HasKey(e => e.Identity);

            builder.Property(p => p.CustomsOutNo).HasMaxLength(50);
            builder.HasOne(w => w.GarmentSubconCustomsInItem)
                .WithMany(h => h.GarmentSubconCustomsInDetail)
                .HasForeignKey(f => f.SubconCustomsInItemId);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
