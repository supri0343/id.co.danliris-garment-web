using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Manufactures.Domain.GarmentCuttingIns.ReadModels
{
    public class GarmentCuttingInItemReadModel : ReadModelBase
    {
        public GarmentCuttingInItemReadModel(Guid identity) : base(identity)
        {
        }

        public Guid CutInId { get; internal set; }
        public Guid PreparingId { get; internal set; }
        public int UENId { get; internal set; }
        [MaxLength(100)]
        public string UENNo { get; internal set; }
    }
}
