using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Manufactures.Domain.GarmentCuttingIns.ReadModels
{
    public class GarmentCuttingInReadModel : ReadModelBase
    {
        public GarmentCuttingInReadModel(Guid identity) : base(identity)
        {
        }

        [MaxLength(25)]
        public string CutInNo { get; internal set; }
        [MaxLength(25)]
        public string CuttingType { get; internal set; }
        [MaxLength(25)]
        public string RONo { get; internal set; }
        [MaxLength(50)]
        public string Article { get; internal set; }
        public int UnitId { get; internal set; }
        [MaxLength(25)]
        public string UnitCode { get; internal set; }
        [MaxLength(100)]
        public string UnitName { get; internal set; }
        public DateTimeOffset CuttingInDate { get; internal set; }
        public double FC { get; internal set; }
    }
}
