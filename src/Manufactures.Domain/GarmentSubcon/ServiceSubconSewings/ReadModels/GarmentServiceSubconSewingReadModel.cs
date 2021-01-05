using System;
using System.Collections.Generic;
using Infrastructure.Domain.ReadModels;
using Manufactures.Domain.GarmentSewingIns.ReadModels;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ReadModels
{
    public class GarmentServiceSubconSewingReadModel : ReadModelBase
    {
        public GarmentServiceSubconSewingReadModel(Guid identity) : base(identity)
        {
        }

        public string ServiceSubconSewingNo { get; internal set; }
        public int BuyerId { get; internal set; }
        public string BuyerCode { get; internal set; }
        public string BuyerName { get; internal set; }
        public int UnitId { get; internal set; }
        public string UnitCode { get; internal set; }
        public string UnitName { get; internal set; }
        public string RONo { get; internal set; }
        public string Article { get; internal set; }
        public int ComodityId { get; internal set; }
        public string ComodityCode { get; internal set; }
        public string ComodityName { get; internal set; }
        public DateTimeOffset ServiceSubconSewingDate { get; internal set; }
        public string UId { get; private set; }
        public virtual List<GarmentServiceSubconSewingItemReadModel> GarmentServiceSubconSewingItem { get; internal set; }

    }
}
