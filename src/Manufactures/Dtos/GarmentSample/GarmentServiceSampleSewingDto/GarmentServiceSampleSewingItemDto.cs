using System;
using System.Collections.Generic;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Dtos.GarmentSample
{
    public class GarmentServiceSampleSewingItemDto : BaseDto
    {
        public GarmentServiceSampleSewingItemDto(GarmentServiceSampleSewingItem garmentServiceSampleSewingItem)
        {
            Id = garmentServiceSampleSewingItem.Identity;
            ServiceSampleSewingId = garmentServiceSampleSewingItem.ServiceSampleSewingId;
            RONo = garmentServiceSampleSewingItem.RONo;
            Article = garmentServiceSampleSewingItem.Article;
            Comodity = new GarmentComodity(garmentServiceSampleSewingItem.ComodityId.Value, garmentServiceSampleSewingItem.ComodityCode, garmentServiceSampleSewingItem.ComodityName);
            Buyer = new Buyer(garmentServiceSampleSewingItem.BuyerId.Value, garmentServiceSampleSewingItem.BuyerCode, garmentServiceSampleSewingItem.BuyerName);
            Unit = new UnitDepartment(garmentServiceSampleSewingItem.UnitId.Value, garmentServiceSampleSewingItem.UnitCode, garmentServiceSampleSewingItem.UnitName);

            Details = new List<GarmentServiceSampleSewingDetailDto>();
        }

        public Guid Id { get; set; }
        public Guid ServiceSampleSewingId { get; set; }
        public Buyer Buyer { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public GarmentComodity Comodity { get; set; }
        public UnitDepartment Unit { get; set; }
        public virtual List<GarmentServiceSampleSewingDetailDto> Details { get; internal set; }
    }
}
