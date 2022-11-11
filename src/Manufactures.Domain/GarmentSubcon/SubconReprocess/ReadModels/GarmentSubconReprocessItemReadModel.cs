using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.SubconReprocess.ReadModels
{
    public class GarmentSubconReprocessItemReadModel : ReadModelBase
    {
        public GarmentSubconReprocessItemReadModel(Guid identity) : base(identity)
        {
        }

        public Guid ReprocessId { get; internal set; }
        //WASH/SEWING
        public Guid ServiceSubconSewingId { get; internal set; }
        public string ServiceSubconSewingNo { get; internal set; }
        public Guid ServiceSubconSewingItemId { get; internal set; }

        //KOMPONEN/CUTTING
        public Guid ServiceSubconCuttingId { get; internal set; }
        public string ServiceSubconCuttingNo { get; internal set; }
        public Guid ServiceSubconCuttingItemId { get; internal set; }

        public string RONo { get; internal set; }
        public string Article { get; internal set; }
        public int ComodityId { get; internal set; }
        public string ComodityCode { get; internal set; }
        public string ComodityName { get; internal set; }
        public int BuyerId { get; internal set; }
        public string BuyerCode { get; internal set; }
        public string BuyerName { get; internal set; }

        public virtual GarmentSubconReprocessReadModel GarmentSubconReprocess { get; internal set; }
        public virtual List<GarmentSubconReprocessDetailReadModel> GarmentSubconReprocessDetail { get; internal set; }
    }
}
