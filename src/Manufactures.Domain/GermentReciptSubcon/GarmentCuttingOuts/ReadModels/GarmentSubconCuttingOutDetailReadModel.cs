using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentCuttingOuts.ReadModels
{
    public class GarmentSubconCuttingOutDetailReadModel : ReadModelBase
    {
        public GarmentSubconCuttingOutDetailReadModel(Guid identity) : base(identity)
        {
        }

        public Guid CutOutItemId { get; internal set; }
        public int SizeId { get; internal set; }
        public string SizeName { get; internal set; }
        public double CuttingOutQuantity { get; internal set; }
        public int CuttingOutUomId { get; internal set; }
        public string CuttingOutUomUnit { get; internal set; }
        public string Color { get; internal set; }
        public double RealQtyOut { get; internal set; }
        public double BasicPrice { get; internal set; }
        public double Price { get; internal set; }
        public string Remark { get; internal set; }
		public string UId { get; private set; }
		public virtual GarmentSubconCuttingOutItemReadModel GarmentSubconCuttingOutItemIdentity { get; internal set; }

    }
}
