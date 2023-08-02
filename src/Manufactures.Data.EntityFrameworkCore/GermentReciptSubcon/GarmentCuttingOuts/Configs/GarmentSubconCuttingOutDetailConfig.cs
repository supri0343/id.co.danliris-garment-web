using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingOuts.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Manufactures.Data.EntityFrameworkCore.GermentReciptSubcon.GarmentCuttingOuts.Configs
{
    public class GarmentSubconCuttingOutDetailConfig : IEntityTypeConfiguration<GarmentSubconCuttingOutDetailReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSubconCuttingOutDetailReadModel> builder)
        {
            builder.ToTable("GarmentSubconCuttingOutDetails");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentSubconCuttingOutItemIdentity)
               .WithMany(a => a.GarmentSubconCuttingOutDetail)
               .HasForeignKey(a => a.CutOutItemId);

            builder.Property(a => a.SizeName)
               .HasMaxLength(100);
            builder.Property(a => a.Color)
               .HasMaxLength(1000);
            builder.Property(a => a.CuttingOutUomUnit)
               .HasMaxLength(10);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}