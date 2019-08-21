using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Manufactures.Domain.GarmentCuttingOuts.ReadModels
{
    public class GarmentCuttingOutReadModel : ReadModelBase
    {
        public GarmentCuttingOutReadModel(Guid identity) : base(identity)
        {
        }

        [MaxLength(25)]
        public string CutOutNo { get; internal set; }
        [MaxLength(25)]
        public string CuttingOutType { get; internal set; }

        public int UnitFromId { get; internal set; }
        [MaxLength(25)]
        public string UnitFromCode { get; internal set; }
        [MaxLength(100)]
        public string UnitFromName { get; internal set; }
        public DateTimeOffset CuttingOutDate { get; internal set; }
        [MaxLength(25)]
        public string RONo { get; internal set; }
        [MaxLength(50)]
        public string Article { get; internal set; }
        public int UnitId { get; internal set; }
        [MaxLength(25)]
        public string UnitCode { get; internal set; }
        [MaxLength(100)]
        public string UnitName { get; internal set; }
        [MaxLength(100)]
        public string Comodity { get; internal set; }

    }
}
