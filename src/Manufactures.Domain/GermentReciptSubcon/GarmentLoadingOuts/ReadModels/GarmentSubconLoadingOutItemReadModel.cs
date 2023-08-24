using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentLoadingOuts.ReadModels
{
    public class GarmentSubconLoadingOutItemReadModel : ReadModelBase
    {
        public GarmentSubconLoadingOutItemReadModel(Guid identity) : base(identity)
        {

        }

        public Guid LoadingOutId { get; internal set; }
        //public Guid SewingDOItemId { get; internal set; }
        public Guid LoadingInItemId { get; internal set; }

        public int ProductId { get; internal set; }
        public string ProductCode { get; internal set; }
        public string ProductName { get; internal set; }
        public string DesignColor { get; internal set; }
        public int SizeId { get; internal set; }
        public string SizeName { get; internal set; }
        public double Quantity { get; internal set; }
        public int UomId { get; internal set; }
        public string UomUnit { get; internal set; }
        public string Color { get; internal set; }
        public double RealQtyOut { get; internal set; }
        public double BasicPrice { get; internal set; }
        public double Price { get; internal set; }
		public string UId { get; private set; }
		public virtual GarmentSubconLoadingOutReadModel GarmentLoading { get; internal set; }

    }
}
