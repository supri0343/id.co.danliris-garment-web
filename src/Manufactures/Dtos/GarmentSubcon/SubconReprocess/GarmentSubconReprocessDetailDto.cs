using Manufactures.Domain.GarmentSubcon.SubconReprocess;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSubcon.SubconReprocess
{
    public class GarmentSubconReprocessDetailDto : BaseDto
    {
        public GarmentSubconReprocessDetailDto(GarmentSubconReprocessDetail garmentSubconReprocess)
        {
            Id = garmentSubconReprocess.Identity;
            ReprocessItemId = garmentSubconReprocess.ReprocessItemId;
            Size = new SizeValueObject(garmentSubconReprocess.SizeId.Value, garmentSubconReprocess.SizeName);
            Quantity = garmentSubconReprocess.Quantity;
            ReprocessQuantity = garmentSubconReprocess.ReprocessQuantity;
            Uom = new Uom(garmentSubconReprocess.UomId.Value, garmentSubconReprocess.UomUnit);
            Quantity = garmentSubconReprocess.Quantity;
            CreatedBy = garmentSubconReprocess.AuditTrail.CreatedBy;
            Color = garmentSubconReprocess.Color;
            DesignColor = garmentSubconReprocess.DesignColor;
            ServiceSubconCuttingDetailId = garmentSubconReprocess.ServiceSubconCuttingDetailId;
            ServiceSubconCuttingSizeId = garmentSubconReprocess.ServiceSubconCuttingSizeId;
            ServiceSubconSewingDetailId = garmentSubconReprocess.ServiceSubconSewingDetailId;
            Unit = new UnitDepartment(garmentSubconReprocess.UnitId.Value, garmentSubconReprocess.UnitCode, garmentSubconReprocess.UnitName);
            Remark = garmentSubconReprocess.Remark;
        }

        public Guid Id { get; set; }
        public Guid ReprocessItemId { get; set; }
        public SizeValueObject Size { get; set; }
        public double Quantity { get; set; }
        public double ReprocessQuantity { get; set; }
        public Uom Uom { get; set; }
        public string Color { get; set; }
        public string DesignColor { get; set; }

        //KOMPONEN (CUTTING)
        public Guid ServiceSubconCuttingDetailId { get; set; }
        public Guid ServiceSubconCuttingSizeId { get; set; }

        //WASH (SEWING)
        public Guid ServiceSubconSewingDetailId { get; set; }
        public UnitDepartment Unit { get; set; }
        public string Remark { get; set; }
    }
}
