using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.SubconReprocess.ReadModels
{
    public class GarmentSubconReprocessDetailReadModel : ReadModelBase
    {
        public GarmentSubconReprocessDetailReadModel(Guid identity) : base(identity)
        {
        }


        public Guid ReprocessItemId { get; internal set; }
        public int SizeId { get; internal set; }
        public string SizeName { get; internal set; }
        public double Quantity { get; internal set; }
        public double ReprocessQuantity { get; internal set; }
        public int UomId { get; internal set; }
        public string UomUnit { get; internal set; }
        public string Color { get; internal set; }
        public string DesignColor { get; internal set; }

        //KOMPONEN (CUTTING)
        public Guid ServiceSubconCuttingDetailId { get; internal set; }
        public Guid ServiceSubconCuttingSizeId { get; internal set; }

        //WASH (SEWING)
        public Guid ServiceSubconSewingDetailId { get; internal set; }

        public int UnitId { get; internal set; }
        public string UnitCode { get; internal set; }
        public string UnitName { get; internal set; }
        public string Remark { get; internal set; }

        public virtual GarmentSubconReprocessItemReadModel GarmentSubconReprocessItem { get; internal set; }
    }
}
