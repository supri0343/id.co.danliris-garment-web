using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Manufactures.Domain.GarmentCuttingOuts.ReadModels
{
    public class GarmentCuttingOutItemReadModel : ReadModelBase
    {
        public GarmentCuttingOutItemReadModel(Guid identity) : base(identity)
        {
        }

        public Guid CuttingInId { get; internal set; }
        public Guid CutOutId { get; internal set; }
        public int ProductId { get; internal set; }
        [MaxLength(100)]
        public string ProductCode { get; internal set; }
        [MaxLength(100)]
        public string ProductName { get; internal set; }
        [MaxLength(100)]
        public string DesignColor { get; internal set; }
        public double TotalCuttingOut { get; internal set; }
        public double RemainingQuantity { get; internal set; }

    }
}
