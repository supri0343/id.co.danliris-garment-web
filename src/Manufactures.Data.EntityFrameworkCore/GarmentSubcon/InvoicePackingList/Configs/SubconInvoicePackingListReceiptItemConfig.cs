using Manufactures.Domain.GarmentSubcon.InvoicePackingList.ReadModels;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList.SubconInvoicePackingListReceiptItemReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;


namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.InvoicePackingList.ReceiptConfigs
{
    public class SubconInvoicePackingListReceiptItemConfig : IEntityTypeConfiguration<SubconInvoicePackingListReceiptItemReadModel>
    {
        public void Configure(EntityTypeBuilder<SubconInvoicePackingListReceiptItemReadModel> builder)
        {
            builder.ToTable("SubconInvoicePackingListReceiptItems");
            builder.HasKey(e => e.Identity);
            
            builder.Property(p => p.DLNo).HasMaxLength(50);
            builder.Property(p => p.ProductCode).HasMaxLength(25);
            builder.Property(p => p.ProductName).HasMaxLength(100);
            builder.Property(p => p.ProductRemark).HasMaxLength(255);

            //builder.Property(p => p.DesignColor).HasMaxLength(255);
            builder.Property(p => p.UomUnit).HasMaxLength(50);

            builder.HasOne(w => w.SubconInvoicePacking)
                .WithMany(m => m.SubconInvoicePackingListReceiptItem)
                .HasForeignKey(s => s.InvoicePackingListId);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();


        }
    }
}
