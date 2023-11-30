using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingOuts.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Manufactures.Data.EntityFrameworkCore.GermentReciptSubcon.GarmentCuttingOuts.Configs
{
    public class GarmentSubconCuttingOutConfig : IEntityTypeConfiguration<GarmentSubconCuttingOutReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSubconCuttingOutReadModel> builder)
        {
            builder.ToTable("GarmentSubconCuttingOuts");
            builder.HasKey(e => e.Identity);

            builder.Property(a => a.CutOutNo).HasMaxLength(25);
            builder.Property(a => a.CuttingOutType).HasMaxLength(25);
            builder.Property(a => a.UnitFromCode).HasMaxLength(25);
            builder.Property(a => a.UnitFromName).HasMaxLength(100);
            builder.Property(a => a.RONo).HasMaxLength(25);
            builder.Property(a => a.Article).HasMaxLength(50);
            builder.Property(a => a.UnitCode).HasMaxLength(25);
            builder.Property(a => a.UnitName).HasMaxLength(100);
            builder.Property(a => a.ComodityCode).HasMaxLength(25);
            builder.Property(a => a.ComodityName).HasMaxLength(100);
            builder.Property(a => a.POSerialNumber)
               .HasMaxLength(100);

            builder.HasIndex(i => i.CutOutNo).IsUnique().HasFilter("[Deleted]=(0)");

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
