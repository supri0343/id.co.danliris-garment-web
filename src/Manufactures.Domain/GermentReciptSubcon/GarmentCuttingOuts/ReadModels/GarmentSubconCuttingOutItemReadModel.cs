using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentCuttingOuts.ReadModels
{
    public class GarmentSubconCuttingOutItemReadModel : ReadModelBase
    {
        public GarmentSubconCuttingOutItemReadModel(Guid identity) : base(identity)
        {
        }

        public Guid CutOutId { get; internal set; }
        public Guid CuttingInId { get; internal set; }
        public Guid CuttingInDetailId { get; internal set; }
        public int ProductId { get; internal set; }
        public string ProductCode { get; internal set; }
        public string ProductName { get; internal set; }
        public string DesignColor { get; internal set; }
        public double TotalCuttingOut { get; internal set; }
		public string UId { get; private set; }
        public double RealQtyOut { get; internal set; }
        public virtual ICollection<GarmentSubconCuttingOutDetailReadModel> GarmentSubconCuttingOutDetail { get; internal set; }
        public virtual GarmentSubconCuttingOutReadModel GarmentSubconCuttingOutIdentity { get; internal set; }
    }
}
