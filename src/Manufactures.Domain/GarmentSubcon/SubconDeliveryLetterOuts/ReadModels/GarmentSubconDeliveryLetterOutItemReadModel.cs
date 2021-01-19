using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.ReadModels
{
    public class GarmentSubconDeliveryLetterOutItemReadModel : ReadModelBase
    {
        public GarmentSubconDeliveryLetterOutItemReadModel(Guid identity) : base(identity)
        {
        }
        public Guid SubconDeliveryLetterOutId { get; internal set; }
        public Guid UENItemId { get; internal set; }

        public int ProductId { get; internal set; }
        public string ProductCode { get; internal set; }
        public string ProductName { get; internal set; }
        public string ProductRemark { get; internal set; }

        public string DesignColor { get; internal set; }
        public double Quantity { get; internal set; }

        public int UomId { get; internal set; }
        public string UomUnit { get; internal set; }

        public string FabricType { get; internal set; }
        public virtual GarmentSubconDeliveryLetterOutReadModel GarmentSubconDeliveryLetterOut { get; internal set; }
    }
}
