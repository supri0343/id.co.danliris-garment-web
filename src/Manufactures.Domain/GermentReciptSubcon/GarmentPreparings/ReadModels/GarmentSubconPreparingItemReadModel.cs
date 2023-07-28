using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentPreparings.ReadModels
{
    public class GarmentSubconPreparingItemReadModel : ReadModelBase
    {
        public GarmentSubconPreparingItemReadModel(Guid identity) : base(identity)
        {

        }

        public int UENItemId { get; internal set; }
        public int ProductId { get; internal set; }
        public string ProductCode { get; internal set; }
        public string ProductName { get; internal set; }
        public string DesignColor { get; internal set; }
        public double Quantity { get; internal set; }
        public int UomId { get; internal set; }
        public string UomUnit { get; internal set; }
        public string FabricType { get; internal set; }
        public double RemainingQuantity { get; internal set; }
        public double BasicPrice { get; internal set; }
        public Guid GarmentSubconPreparingId { get; internal set; }
		public string UId { get; private set; }
		public virtual GarmentSubconPreparingReadModel GarmentPreparingIdentity { get; internal set; }


        public string ROSource { get; internal set; }
        public string BeacukaiNo { get; internal set; }
        public DateTimeOffset BeacukaiDate { get; internal set; }
        public string BeacukaiType { get; internal set; }
    }
}