using Manufactures.Domain.GarmentPreparings.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentPreparings.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Manufactures.Data.EntityFrameworkCore.GermentReciptSubcon.GarmentPreparings.Config
{
    public class GarmentSubconPreparingConfig : IEntityTypeConfiguration<GarmentSubconPreparingReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSubconPreparingReadModel> builder)
        {
            builder.ToTable("GarmentSubconPreparings");
            builder.HasKey(e => e.Identity);

            builder.Property(o => o.UENNo)
               .HasMaxLength(100);
            builder.Property(o => o.UnitCode)
               .HasMaxLength(25);
            builder.Property(o => o.UnitName)
               .HasMaxLength(100);
            builder.Property(o => o.RONo)
               .HasMaxLength(100);
            builder.Property(o => o.Article)
               .HasMaxLength(500);
            builder.Property(o => o.ProductOwnerCode)
               .HasMaxLength(100);
            builder.Property(o => o.ProductOwnerName)
               .HasMaxLength(500);
			builder.Property(o => o.UId)
				.HasMaxLength(255);
            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}