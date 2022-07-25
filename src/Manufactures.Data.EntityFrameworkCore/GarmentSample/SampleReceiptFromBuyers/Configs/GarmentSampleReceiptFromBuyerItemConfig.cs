using Manufactures.Domain.GarmentSample.SampleReceiptFromBuyers.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleReceiptFromBuyers.Configs
{
	public class GarmentSampleReceiptFromBuyerItemConfig : IEntityTypeConfiguration<GarmentSampleReceiptFromBuyerItemReadModel>
	{
		public void Configure(EntityTypeBuilder<GarmentSampleReceiptFromBuyerItemReadModel> builder)
		{
			builder.ToTable("GarmentSampleReceiptFromBuyerItems");
			builder.HasKey(e => e.Identity);
			builder.HasOne(a => a.GarmentSampleReceiptFromBuyer)
				  .WithMany(a => a.Items)
				  .HasForeignKey(a => a.ReceiptId);

			builder.Property(a => a.SizeName).HasMaxLength(100);
			builder.Property(a => a.ComodityCode).HasMaxLength(50);
			builder.Property(a => a.ComodityName).HasMaxLength(100);
			builder.ApplyAuditTrail();
			builder.ApplySoftDelete();
		}
	}
}

