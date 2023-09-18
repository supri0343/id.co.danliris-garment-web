using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentSewingOuts.ReadModels
{
    public class GarmentSubconSewingOutDetailReadModel : ReadModelBase
    {
        public GarmentSubconSewingOutDetailReadModel(Guid identity) : base(identity)
        {
        }

        public Guid SewingOutItemId { get; internal set; }
        public int SizeId { get; internal set; }
        public string SizeName { get; internal set; }
        public double Quantity { get; internal set; }
        public int UomId { get; internal set; }
        public string UomUnit { get; internal set; }
        public double RealQtyOut { get; internal set; }

        public virtual GarmentSubconSewingOutItemReadModel GarmentSewingOutItemIdentity { get; internal set; }

    }
}
