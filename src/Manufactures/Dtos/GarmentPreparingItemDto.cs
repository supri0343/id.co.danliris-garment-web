using Manufactures.Domain.GarmentPreparings;
using Manufactures.Domain.GarmentPreparings.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos
{
    public class GarmentPreparingItemDto
    {
        public GarmentPreparingItemDto(GarmentPreparingItem garmentPreparingItem)
        {
            Id = garmentPreparingItem.Identity;

            LastModifiedDate = garmentPreparingItem.AuditTrail.ModifiedDate ?? garmentPreparingItem.AuditTrail.CreatedDate;
            LastModifiedBy = garmentPreparingItem.AuditTrail.ModifiedBy ?? garmentPreparingItem.AuditTrail.CreatedBy;
            PreparingId = garmentPreparingItem.PreparingId;
            UENItemId = garmentPreparingItem.UENItemId;
            ProductId = new Product(garmentPreparingItem.ProductId.Value, "", "");
            DesignColor = garmentPreparingItem.DesignColor;
            Quantity = garmentPreparingItem.Quantity;
            UomId = new Uom(garmentPreparingItem.UomId.Value, "");
            FabricType = garmentPreparingItem.FabricType;
            RemainingQuantity = garmentPreparingItem.RemainingQuantity;
            BasicPrice = garmentPreparingItem.BasicPrice;
            GarmentPreparingId = garmentPreparingItem.GarmentPreparingId;
        }

        public Guid Id { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTimeOffset LastModifiedDate { get; set; }

        public int PreparingId { get; set; }
        public int UENItemId { get; set; }
        public Product ProductId { get; set; }
        public string DesignColor { get; set; }
        public double Quantity { get; set; }
        public Uom UomId { get; set; }
        public string FabricType { get; set; }
        public double RemainingQuantity { get; set; }
        public double BasicPrice { get; set; }
        public Guid GarmentPreparingId { get; set; }
    }
}