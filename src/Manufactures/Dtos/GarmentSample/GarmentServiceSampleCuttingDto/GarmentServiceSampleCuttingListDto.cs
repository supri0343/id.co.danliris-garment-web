using Manufactures.Domain.GarmentSample.ServiceSampleCuttings;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSample
{
    public class GarmentServiceSampleCuttingListDto : BaseDto
    {
        public GarmentServiceSampleCuttingListDto(GarmentServiceSampleCutting garmentServiceSampleCutting)
        {
            Id = garmentServiceSampleCutting.Identity;
            SubconNo = garmentServiceSampleCutting.SampleNo;
            SubconType = garmentServiceSampleCutting.SampleType;
            SubconDate = garmentServiceSampleCutting.SampleDate;
            CreatedBy = garmentServiceSampleCutting.AuditTrail.CreatedBy;
            Buyer = new Buyer(garmentServiceSampleCutting.BuyerId.Value, garmentServiceSampleCutting.BuyerCode, garmentServiceSampleCutting.BuyerName);
            Unit = new UnitDepartment(garmentServiceSampleCutting.UnitId.Value, garmentServiceSampleCutting.UnitCode, garmentServiceSampleCutting.UnitName);
            IsUsed = garmentServiceSampleCutting.IsUsed;
            Items = new List<GarmentServiceSampleCuttingItemDto>();
        }

        public Guid Id { get; set; }
        public string SubconNo { get; set; }
        public string SubconType { get; set; }

        public DateTimeOffset SubconDate { get; set; }
        public UnitDepartment Unit { get; set; }

        public double TotalQuantity { get; set; }
        public bool IsUsed { get; set; }
        public List<string> Products { get; set; }
        public Buyer Buyer { get; set; }
        public List<GarmentServiceSampleCuttingItemDto> Items { get; set; }
    }
}