using Manufactures.Domain.GarmentSewingIns.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingIns.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Manufactures.Data.EntityFrameworkCore.GermentReciptSubcon.GarmentSewingIns.Configs
{
    public class GarmentSubconSewingInConfig : IEntityTypeConfiguration<GarmentSubconSewingInReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSubconSewingInReadModel> builder)
        {
            builder.ToTable("GarmentSubconSewingIns");
            builder.HasKey(e => e.Identity);

            builder.Property(a => a.SewingInNo).HasMaxLength(25);
            builder.Property(a => a.LoadingOutNo).HasMaxLength(25);
            builder.Property(a => a.SewingFrom).HasMaxLength(25);
            builder.Property(a => a.UnitFromCode).HasMaxLength(25);
            builder.Property(a => a.UnitFromName).HasMaxLength(100);
            builder.Property(a => a.UnitCode).HasMaxLength(25);
            builder.Property(a => a.UnitName).HasMaxLength(100);
            builder.Property(a => a.RONo).HasMaxLength(25);
            builder.Property(a => a.Article).HasMaxLength(50);
            builder.Property(a => a.ComodityCode).HasMaxLength(25);
            builder.Property(a => a.ComodityName).HasMaxLength(100);

            builder.HasIndex(i => i.SewingInNo).IsUnique().HasFilter("[Deleted]=(0)");

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}