using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingOuts.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Manufactures.Data.EntityFrameworkCore.GermentReciptSubcon.GarmentCuttingOuts.Configs
{
    public class GarmentSubconCuttingOutItemConfig : IEntityTypeConfiguration<GarmentSubconCuttingOutItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSubconCuttingOutItemReadModel> builder)
        {
            builder.ToTable("GarmentSubconCuttingOutItems");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentSubconCuttingOutIdentity)
                   .WithMany(a => a.GarmentSubconCuttingOutItem)
                   .HasForeignKey(a => a.CutOutId);

            builder.Property(a => a.ProductCode)
               .HasMaxLength(25);
            builder.Property(a => a.ProductName)
               .HasMaxLength(100);
            builder.Property(a => a.DesignColor)
               .HasMaxLength(2000);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}