using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.ReadModels
{
    public class GarmentSubconDeliveryLetterOutDetailReadModel : ReadModelBase
    {
        public GarmentSubconDeliveryLetterOutDetailReadModel(Guid identity) : base(identity)
        {
        }
        public Guid SubconDeliveryLetterOutItemId { get; internal set; }
        public int UENItemId { get; internal set; }

        public int ProductId { get; internal set; }
        public string ProductCode { get; internal set; }
        public string ProductName { get; internal set; }
        public string ProductRemark { get; internal set; }

        public string DesignColor { get; internal set; }
        public double Quantity { get; internal set; }

        public int UomId { get; internal set; }
        public string UomUnit { get; internal set; }
        public int UomOutId { get; internal set; }
        public string UomOutUnit { get; internal set; }

        public string FabricType { get; internal set; }

        public int UENId { get; internal set; }
        public string UENNo { get; internal set; }
        public virtual GarmentSubconDeliveryLetterOutItemReadModel GarmentSubconDeliveryLetterOutItem { get; internal set; }
    }
}
