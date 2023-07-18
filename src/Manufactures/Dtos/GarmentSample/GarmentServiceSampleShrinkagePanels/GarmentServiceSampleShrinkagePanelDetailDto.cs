using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels;
using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Dtos.GarmentSample.GarmentServiceSampleShrinkagePanels
{
    public class GarmentServiceSampleShrinkagePanelDetailDto
    {
        public GarmentServiceSampleShrinkagePanelDetailDto(GarmentServiceSampleShrinkagePanelDetail garmentServiceSampleShrinkagePanelDetail)
        {
            Id = garmentServiceSampleShrinkagePanelDetail.Identity;
            ServiceSampleShrinkagePanelItemId = garmentServiceSampleShrinkagePanelDetail.ServiceSampleShrinkagePanelItemId;
            Product = new Product(garmentServiceSampleShrinkagePanelDetail.ProductId.Value, garmentServiceSampleShrinkagePanelDetail.ProductCode, garmentServiceSampleShrinkagePanelDetail.ProductName, garmentServiceSampleShrinkagePanelDetail.ProductRemark);
            DesignColor = garmentServiceSampleShrinkagePanelDetail.DesignColor;
            Quantity = garmentServiceSampleShrinkagePanelDetail.Quantity;
            Uom = new Uom(garmentServiceSampleShrinkagePanelDetail.UomId.Value, garmentServiceSampleShrinkagePanelDetail.UomUnit);
        }

        public Guid Id { get; set; }
        public Guid ServiceSampleShrinkagePanelItemId { get; set; }
        public Product Product { get; set; }
        public string DesignColor { get; set; }
        public decimal Quantity { get; set; }
        public Uom Uom { get; set; }
        public string Composition { get; set; }
    }
}
