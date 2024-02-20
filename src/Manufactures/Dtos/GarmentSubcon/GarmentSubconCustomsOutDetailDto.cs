
using Manufactures.Domain.GarmentSubcon.CustomsOuts;
using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Dtos.GarmentSubcon
{
    public class GarmentSubconCustomsOutDetailDto : BaseDto
    {
        public GarmentSubconCustomsOutDetailDto(GarmentSubconCustomsOutDetail garmentSubconCustomsOutDetail)
        {
            Id = garmentSubconCustomsOutDetail.Identity;
            Product = new Product(garmentSubconCustomsOutDetail.ProductId.Value, garmentSubconCustomsOutDetail.ProductCode, garmentSubconCustomsOutDetail.ProductName, garmentSubconCustomsOutDetail.ProductRemark);
            Quantity = garmentSubconCustomsOutDetail.Quantity;
            Uom = new Uom(garmentSubconCustomsOutDetail.UomId.Value, garmentSubconCustomsOutDetail.UomUnit);
        }

        public Guid Id { get; set; }
        public Product Product { get; set; }
        public string ProductRemark { get; set; }
        public double Quantity { get; set; }
        public Uom Uom { get; set; }
    }
}
