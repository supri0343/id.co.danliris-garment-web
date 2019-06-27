using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Manufactures.Domain.GarmentCuttingIns.ReadModels
{
    public class GarmentCuttingInDetailReadModel : ReadModelBase
    {
        public GarmentCuttingInDetailReadModel(Guid identity) : base(identity)
        {
        }

        public Guid CutInItemId { get; internal set; }
        public Guid PreparingItemId { get; internal set; }

        public int ProductId { get; internal set; }
        [MaxLength(25)]
        public string ProductCode { get; internal set; }
        [MaxLength(100)]
        public string ProductName { get; internal set; }

        [MaxLength(25)]
        public string DesignColor { get; internal set; }
        [MaxLength(25)]
        public string FabricType { get; internal set; }

        public double PreparingQuantity { get; internal set; }
        public int PreparingUomId { get; internal set; }
        [MaxLength(10)]
        public string PreparingUomUnit { get; internal set; }

        public double CuttingInQuantity { get; internal set; }
        public int CuttingInUomId { get; internal set; }
        [MaxLength(10)]
        public string CuttingInUomUnit { get; internal set; }

        public double RemainingQuantity { get; internal set; }
        public double BasicPrice { get; internal set; }
    }
}
