using Manufactures.Domain.GarmentSample.SampleReceiptFromBuyers.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleReceiptFromBuyers.Configs
{
	public class GarmentSampleReceiptFromBuyerConfig : IEntityTypeConfiguration<GarmentSampleReceiptFromBuyerReadModel>
	{
		public void Configure(EntityTypeBuilder<GarmentSampleReceiptFromBuyerReadModel> builder)
		{
			builder.ToTable("GarmentSampleReceiptFromBuyers");
			builder.HasKey(e => e.Identity);
			builder.HasIndex(i => i.ReceiptNo).IsUnique().HasFilter("[Deleted]=(0)");
			builder.Property(a => a.ReceiptNo).HasMaxLength(25);
			builder.ApplyAuditTrail();
			builder.ApplySoftDelete();
		}
	}
}
