using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts;
using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Dtos.GarmentSubcon
{
    public class GarmentSubconDeliveryLetterOutDetailDto
    {
        public GarmentSubconDeliveryLetterOutDetailDto(GarmentSubconDeliveryLetterOutDetail garmentSubconDeliveryLetterOutDetail)
        {
            Id = garmentSubconDeliveryLetterOutDetail.Identity;
            SubconDeliveryLetterOutItemId = garmentSubconDeliveryLetterOutDetail.SubconDeliveryLetterOutItemId;
            UENItemId = garmentSubconDeliveryLetterOutDetail.UENItemId;
            Product = new Product(garmentSubconDeliveryLetterOutDetail.ProductId.Value, garmentSubconDeliveryLetterOutDetail.ProductCode, garmentSubconDeliveryLetterOutDetail.ProductName, garmentSubconDeliveryLetterOutDetail.ProductRemark);
            DesignColor = garmentSubconDeliveryLetterOutDetail.DesignColor;
            Quantity = garmentSubconDeliveryLetterOutDetail.Quantity;
            Uom = new Uom(garmentSubconDeliveryLetterOutDetail.UomId.Value, garmentSubconDeliveryLetterOutDetail.UomUnit);
            UomOut = new Uom(garmentSubconDeliveryLetterOutDetail.UomOutId.Value, garmentSubconDeliveryLetterOutDetail.UomOutUnit);
            FabricType = garmentSubconDeliveryLetterOutDetail.FabricType;
            UENId = garmentSubconDeliveryLetterOutDetail.UENId;
            UENNo = garmentSubconDeliveryLetterOutDetail.UENNo;
        }

        public Guid Id { get; set; }
        public Guid SubconDeliveryLetterOutItemId { get; set; }
        public int UENItemId { get; set; }

        public Product Product { get; set; }
        public string ProductRemark { get; set; }

        public string DesignColor { get; set; }
        public double Quantity { get; set; }

        public Uom Uom { get; set; }
        public Uom UomOut { get; set; }

        public string FabricType { get; set; }
        public int UENId { get; set; }
        public string UENNo { get; set; }
    }
}
