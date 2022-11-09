using Manufactures.Domain.GarmentSubcon.SubconReprocess;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSubcon.SubconReprocess
{
    public class GarmentSubconReprocessItemDto : BaseDto
    {
        public GarmentSubconReprocessItemDto(GarmentSubconReprocessItem garmentSubconReprocessItem)
        {
            Id = garmentSubconReprocessItem.Identity;
            ReprocessId = garmentSubconReprocessItem.ReprocessId;
            ServiceSubconSewingId = garmentSubconReprocessItem.ServiceSubconSewingId;
            ServiceSubconSewingNo = garmentSubconReprocessItem.ServiceSubconSewingNo;
            ServiceSubconSewingItemId = garmentSubconReprocessItem.ServiceSubconSewingItemId;
            ServiceSubconCuttingId = garmentSubconReprocessItem.ServiceSubconCuttingId;
            ServiceSubconCuttingNo = garmentSubconReprocessItem.ServiceSubconCuttingNo;
            ServiceSubconCuttingItemId = garmentSubconReprocessItem.ServiceSubconCuttingItemId;
            RONo = garmentSubconReprocessItem.RONo;
            Article = garmentSubconReprocessItem.Article;
            Buyer = new Buyer(garmentSubconReprocessItem.BuyerId.Value, garmentSubconReprocessItem.BuyerCode, garmentSubconReprocessItem.BuyerName);
            Comodity = new GarmentComodity(garmentSubconReprocessItem.ComodityId.Value, garmentSubconReprocessItem.ComodityCode, garmentSubconReprocessItem.ComodityName);
            Details = new List<GarmentSubconReprocessDetailDto>();
        }

        public Guid Id { get; set; }
        public Guid ReprocessId { get; set; }
        //WASH/SEWING
        public Guid ServiceSubconSewingId { get; set; }
        public string ServiceSubconSewingNo { get;  set; }
        public Guid ServiceSubconSewingItemId { get;  set; }

        //KOMPONEN/CUTTING
        public Guid ServiceSubconCuttingId { get;  set; }
        public string ServiceSubconCuttingNo { get;  set; }
        public Guid ServiceSubconCuttingItemId { get; set; }

        public string RONo { get;  set; }
        public string Article { get;  set; }
        public GarmentComodity Comodity { get;  set; }
        public Buyer Buyer { get; set; }
        public List<GarmentSubconReprocessDetailDto> Details { get; set; }
    }
}
