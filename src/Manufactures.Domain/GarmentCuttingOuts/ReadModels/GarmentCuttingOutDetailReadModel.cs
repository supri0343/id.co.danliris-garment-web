using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Manufactures.Domain.GarmentCuttingOuts.ReadModels
{
    public class GarmentCuttingOutDetailReadModel : ReadModelBase
    {
        public GarmentCuttingOutDetailReadModel(Guid identity) : base(identity)
        {
        }

        public Guid CuttingInItemId { get; internal set; }
        public Guid CutOutItemId { get; internal set; }

        public int SizeId { get; internal set; }
        [MaxLength(100)]
        public string SizeName { get; internal set; }
        [MaxLength(100)]
        public string Color { get; internal set; }

        public double RemainingQuantity { get; internal set; }
        public double CuttingOutQuantity { get; internal set; }
        public double BasicPrice { get; internal set; }
        public double IndirectPrice { get; internal set; }
        public double OTL1 { get; internal set; }
        public double OTL2 { get; internal set; }

        public int CuttingOutUomId { get; internal set; }
        [MaxLength(10)]
        public string CuttingOutUomUnit { get; internal set; }

    }
}
