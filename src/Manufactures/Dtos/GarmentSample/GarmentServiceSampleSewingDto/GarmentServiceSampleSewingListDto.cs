using System;
using System.Collections.Generic;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Dtos.GarmentSample
{
    public class GarmentServiceSampleSewingListDto : BaseDto
    {
        public GarmentServiceSampleSewingListDto(GarmentServiceSampleSewing garmentServiceSampleSewingList)
        {
            Id = garmentServiceSampleSewingList.Identity;
            ServiceSampleSewingNo = garmentServiceSampleSewingList.ServiceSampleSewingNo;
           // Unit = new UnitDepartment(garmentServiceSampleSewingList.UnitId.Value, garmentServiceSampleSewingList.UnitCode, garmentServiceSampleSewingList.UnitName);
            ServiceSampleSewingDate = garmentServiceSampleSewingList.ServiceSampleSewingDate;
            CreatedBy = garmentServiceSampleSewingList.AuditTrail.CreatedBy;
            Buyer = new Buyer(garmentServiceSampleSewingList.BuyerId.Value, garmentServiceSampleSewingList.BuyerCode, garmentServiceSampleSewingList.BuyerName);
            IsUsed = garmentServiceSampleSewingList.IsUsed;
            Items = new List<GarmentServiceSampleSewingItemDto>();
        }

        public Guid Id { get; set; }
        public string ServiceSampleSewingNo { get; set; }
        public UnitDepartment Unit { get; set; }
        public string SewingTo { get; set; }
        public DateTimeOffset ServiceSampleSewingDate { get; set; }
        public double TotalQuantity { get; set; }
        public double TotalRemainingQuantity { get; set; }
        public bool IsUsed { get; set; }
        public Buyer Buyer { get; set; }
        public List<GarmentServiceSampleSewingItemDto> Items { get; set; }
    }
}
