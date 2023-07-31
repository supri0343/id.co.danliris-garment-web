
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingIns.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Manufactures.Data.EntityFrameworkCore.GermentReciptSubcon.GarmentCuttingIns.Configs
{
    public class GarmentSubconCuttingInConfig : IEntityTypeConfiguration<GarmentSubconCuttingInReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSubconCuttingInReadModel> builder)
        {
            builder.ToTable("GarmentSubconCuttingIns");
            builder.HasKey(e => e.Identity);

            builder.Property(p => p.CutInNo).HasMaxLength(25);
            builder.Property(p => p.CuttingType).HasMaxLength(25);
            builder.Property(p => p.CuttingFrom).HasMaxLength(25);
            builder.Property(p => p.RONo).HasMaxLength(25);
            builder.Property(p => p.Article).HasMaxLength(50);
            builder.Property(p => p.UnitCode).HasMaxLength(25);
            builder.Property(p => p.UnitName).HasMaxLength(100);
			builder.Property(p => p.UId).HasMaxLength(255);
			builder.HasIndex(i => i.CutInNo).IsUnique().HasFilter("[Deleted]=(0)");

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
