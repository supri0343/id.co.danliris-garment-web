using Manufactures.Domain.GarmentSample.ServiceSampleCuttings;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSample
{
    public class GarmentServiceSampleCuttingDto : BaseDto
    {
        public GarmentServiceSampleCuttingDto(GarmentServiceSampleCutting garmentServiceSampleCutting)
        {
            Id = garmentServiceSampleCutting.Identity;
            SampleNo = garmentServiceSampleCutting.SampleNo;
            SampleType = garmentServiceSampleCutting.SampleType;
            SampleDate = garmentServiceSampleCutting.SampleDate;
            Unit = new UnitDepartment(garmentServiceSampleCutting.UnitId.Value, garmentServiceSampleCutting.UnitCode, garmentServiceSampleCutting.UnitName);
            CreatedBy = garmentServiceSampleCutting.AuditTrail.CreatedBy;
            IsUsed = garmentServiceSampleCutting.IsUsed;
            Buyer = new Buyer(garmentServiceSampleCutting.BuyerId.Value, garmentServiceSampleCutting.BuyerCode, garmentServiceSampleCutting.BuyerName);
            Uom = new Uom(garmentServiceSampleCutting.UomId.Value, garmentServiceSampleCutting.UomUnit);
            QtyPacking = garmentServiceSampleCutting.QtyPacking;
            NettWeight = garmentServiceSampleCutting.NettWeight;
            GrossWeight = garmentServiceSampleCutting.GrossWeight;
            Remark = garmentServiceSampleCutting.Remark;
            Items = new List<GarmentServiceSampleCuttingItemDto>();
        }

        public Guid Id { get; set; }
        public string SampleNo { get; set; }
        public string SampleType { get; set; }

        public DateTimeOffset SampleDate { get; set; }
        public UnitDepartment Unit { get; set; }
        public bool IsUsed { get; set; }
        public Buyer Buyer { get; set; }
        public Uom Uom { get; set; }
        public int QtyPacking {get; set;}
        public double NettWeight { get; set; }
        public double GrossWeight { get; set; }
        public string Remark { get; set; }
        public List<GarmentServiceSampleCuttingItemDto> Items { get; set; }
    }
}