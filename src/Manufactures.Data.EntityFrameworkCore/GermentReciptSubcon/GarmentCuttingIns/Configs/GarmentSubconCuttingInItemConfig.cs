using Manufactures.Domain.GarmentCuttingIns.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingIns.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Manufactures.Data.EntityFrameworkCore.GermentReciptSubcon.GarmentCuttingIns.Configs
{
    public class GarmentSubconCuttingInItemConfig : IEntityTypeConfiguration<GarmentSubconCuttingInItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSubconCuttingInItemReadModel> builder)
        {
            builder.ToTable("GarmentSubconCuttingInItems");
            builder.HasKey(e => e.Identity);

            builder.Property(p => p.UENNo).HasMaxLength(100);
            builder.Property(p => p.SewingOutNo).HasMaxLength(50);
			builder.Property(p => p.UId).HasMaxLength(255);
			builder.HasOne(w => w.GarmentCuttingIn)
                .WithMany(h => h.Items)
                .HasForeignKey(f => f.CutInId);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
