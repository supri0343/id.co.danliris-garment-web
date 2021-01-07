using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings;
using System;
using Manufactures.Domain.Shared.ValueObjects;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSubcon
{
    public class GarmentServiceSubconCuttingItemDto : BaseDto
    {
        public GarmentServiceSubconCuttingItemDto(GarmentServiceSubconCuttingItem garmentServiceSubconCuttingItem)
        {
            Id = garmentServiceSubconCuttingItem.Identity;
            ServiceSubconCuttingId = garmentServiceSubconCuttingItem.ServiceSubconCuttingId;
            CuttingInDetailId = garmentServiceSubconCuttingItem.CuttingInDetailId;
            Product = new Product(garmentServiceSubconCuttingItem.ProductId.Value, garmentServiceSubconCuttingItem.ProductCode, garmentServiceSubconCuttingItem.ProductName);
            DesignColor = garmentServiceSubconCuttingItem.DesignColor;
        }

        public Guid Id { get; set; }
        public Guid ServiceSubconCuttingId { get; internal set; }
        public Guid CuttingInDetailId { get; internal set; }
        public Product Product { get; set; }

        public string DesignColor { get; set; }

        public double Quantity { get; set; }
    }
}