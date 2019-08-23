using Manufactures.Domain.GarmentCuttingIns.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Manufactures.Data.EntityFrameworkCore.GarmentCuttingIns.Configs
{
    public class GarmentCuttingInItemConfig : IEntityTypeConfiguration<GarmentCuttingInItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentCuttingInItemReadModel> builder)
        {
            builder.ToTable("GarmentCuttingInItems");
            builder.HasKey(e => e.Identity);

            builder.Property(p => p.UENNo).HasMaxLength(100);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
