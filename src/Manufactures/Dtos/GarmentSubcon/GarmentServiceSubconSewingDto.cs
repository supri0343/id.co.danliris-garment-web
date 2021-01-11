using System;
using System.Collections.Generic;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Dtos.GarmentSubcon
{
    public class GarmentServiceSubconSewingDto
    {
        public GarmentServiceSubconSewingDto(GarmentServiceSubconSewing garmentServiceSubconSewingList)
        {
            Id = garmentServiceSubconSewingList.Identity;
            ServiceSubconSewingNo = garmentServiceSubconSewingList.ServiceSubconSewingNo;
            Unit = new UnitDepartment(garmentServiceSubconSewingList.UnitId.Value, garmentServiceSubconSewingList.UnitCode, garmentServiceSubconSewingList.UnitName);
            RONo = garmentServiceSubconSewingList.RONo;
            Article = garmentServiceSubconSewingList.Article;
            ServiceSubconSewingDate = garmentServiceSubconSewingList.ServiceSubconSewingDate;
            Buyer = new Buyer(garmentServiceSubconSewingList.BuyerId.Value, garmentServiceSubconSewingList.BuyerCode, garmentServiceSubconSewingList.BuyerName);
            Comodity = new GarmentComodity(garmentServiceSubconSewingList.ComodityId.Value, garmentServiceSubconSewingList.ComodityCode, garmentServiceSubconSewingList.ComodityName);
            IsDifferentSize = garmentServiceSubconSewingList.IsDifferentSize;

            Items = new List<GarmentServiceSubconSewingItemDto>();
        }

        public Guid Id { get; internal set; }
        public string ServiceSubconSewingNo { get; set; }
        public Buyer Buyer { get; set; }
        public UnitDepartment Unit { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public GarmentComodity Comodity { get; set; }
        public DateTimeOffset ServiceSubconSewingDate { get; set; }
        public bool IsDifferentSize { get; set; }

        public virtual List<GarmentServiceSubconSewingItemDto> Items { get; internal set; }
    }
}
