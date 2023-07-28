using Manufactures.Domain.GarmentPreparings;
using Manufactures.Domain.GarmentPreparings.ValueObjects;
using Manufactures.Domain.GermentReciptSubcon.GarmentPreparings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GermentReciptSubcon.GarmentPreparing
{
    public class GarmentSubconPreparingItemDto
    {
        public GarmentSubconPreparingItemDto(GarmentSubconPreparingItem garmentPreparingItem)
        {
            Id = garmentPreparingItem.Identity;

            LastModifiedDate = garmentPreparingItem.AuditTrail.ModifiedDate ?? garmentPreparingItem.AuditTrail.CreatedDate;
            LastModifiedBy = garmentPreparingItem.AuditTrail.ModifiedBy ?? garmentPreparingItem.AuditTrail.CreatedBy;
            UENItemId = garmentPreparingItem.UENItemId;
            Product = new Product(garmentPreparingItem.ProductId.Value, garmentPreparingItem.ProductName, garmentPreparingItem.ProductCode);
            DesignColor = garmentPreparingItem.DesignColor;
            Quantity = (decimal)garmentPreparingItem.Quantity;
            Uom = new Uom(garmentPreparingItem.UomId.Value, garmentPreparingItem.UomUnit);
            FabricType = garmentPreparingItem.FabricType;
            RemainingQuantity = (decimal)garmentPreparingItem.RemainingQuantity;
            BasicPrice = (decimal)garmentPreparingItem.BasicPrice;
            GarmentPreparingId = garmentPreparingItem.GarmentSubconPreparingId;
            ROSource = garmentPreparingItem.ROSource;
            BeacukaiNo = garmentPreparingItem.BeacukaiNo;
            BeacukaiDate = garmentPreparingItem.BeacukaiDate;
            BeacukaiDate = garmentPreparingItem.BeacukaiDate;
        }

        public Guid Id { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTimeOffset LastModifiedDate { get; set; }

        public int UENItemId { get; set; }
        public Product Product { get; set; }
        public string DesignColor { get; set; }
        public decimal Quantity { get; set; }
        public Uom Uom { get; set; }
        public string FabricType { get; set; }
        public string ROSource { get; set; }
        public decimal RemainingQuantity { get; set; }
        public decimal BasicPrice { get; set; }
        public Guid GarmentPreparingId { get; set; }
        public string BeacukaiNo { get; set; }
        public DateTimeOffset BeacukaiDate { get; set; }
        public string BeacukaiType { get; set; }
    }
}