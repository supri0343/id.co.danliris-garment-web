using Manufactures.Domain.GarmentSubconCuttingOuts;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos
{
    public class GarmentSubconCuttingOutItemDto : BaseDto
    {
        public GarmentSubconCuttingOutItemDto(GarmentSubconCuttingOutItem garmentCuttingOutItem)
        {
            Id = garmentCuttingOutItem.Identity;
            CutOutId = garmentCuttingOutItem.CutOutId;
            CuttingInId = garmentCuttingOutItem.CuttingInId;
            CuttingInDetailId = garmentCuttingOutItem.CuttingInDetailId;
            Product = new Product(garmentCuttingOutItem.ProductId.Value, garmentCuttingOutItem.ProductCode, garmentCuttingOutItem.ProductName);
            DesignColor = garmentCuttingOutItem.DesignColor;
            TotalCuttingOut = garmentCuttingOutItem.TotalCuttingOut;
            TotalCuttingOutQuantity = garmentCuttingOutItem.TotalCuttingOutQuantity;
            EPOId = garmentCuttingOutItem.EPOId;
            EPOItemId = garmentCuttingOutItem.EPOItemId;
            POSerialNumber = garmentCuttingOutItem.POSerialNumber;

            Details = new List<GarmentCuttingOutDetailDto>();
        }

        public Guid Id { get; set; }
        public Guid CutOutId { get; set; }
        public Guid CuttingInId { get; set; }
        public Guid CuttingInDetailId { get; set; }
        public Product Product { get; set; }
        public string DesignColor { get; set; }
        public double TotalCuttingOut { get; set; }
        public double TotalCuttingOutQuantity { get; set; }

        public long EPOId { get; set; }
        public long EPOItemId { get; set; }
        public string POSerialNumber { get; set; }
        public List<GarmentCuttingOutDetailDto> Details { get; set; }
    }
}
