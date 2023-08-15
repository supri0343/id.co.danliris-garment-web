using System;
using System.Collections.Generic;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Dtos.GarmentSample
{
    public class GarmentServiceSampleSewingDto
    {
        public GarmentServiceSampleSewingDto(GarmentServiceSampleSewing garmentServiceSampleSewingList)
        {
            Id = garmentServiceSampleSewingList.Identity;
            ServiceSubconSewingNo = garmentServiceSampleSewingList.ServiceSampleSewingNo;
            //
            ServiceSubconSewingDate = garmentServiceSampleSewingList.ServiceSampleSewingDate;
            IsUsed = garmentServiceSampleSewingList.IsUsed;
            Buyer = new Buyer(garmentServiceSampleSewingList.BuyerId.Value, garmentServiceSampleSewingList.BuyerCode, garmentServiceSampleSewingList.BuyerName);
            QtyPacking = garmentServiceSampleSewingList.QtyPacking;
            UomUnit = garmentServiceSampleSewingList.UomUnit;
            NettWeight = garmentServiceSampleSewingList.NettWeight;
            GrossWeight = garmentServiceSampleSewingList.GrossWeight;
            Items = new List<GarmentServiceSampleSewingItemDto>();
        }

        public Guid Id { get; internal set; }
        public string ServiceSubconSewingNo { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public DateTimeOffset ServiceSubconSewingDate { get; set; }
        public bool IsUsed { get; set; }
        public Buyer Buyer { get; set; }
        public int QtyPacking { get; set; }
        public string UomUnit { get; set; }
        public double NettWeight { get; set; }
        public double GrossWeight { get; set; }
        public virtual List<GarmentServiceSampleSewingItemDto> Items { get; internal set; }
    }
}
