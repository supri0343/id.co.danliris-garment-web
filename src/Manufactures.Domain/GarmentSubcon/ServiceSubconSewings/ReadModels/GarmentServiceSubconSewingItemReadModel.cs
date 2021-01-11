using System;
using Infrastructure.Domain.ReadModels;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ReadModels
{
    public class GarmentServiceSubconSewingItemReadModel : ReadModelBase
    {
        public GarmentServiceSubconSewingItemReadModel(Guid identity) : base(identity)
        {
        }

        public Guid ServiceSubconSewingId { get; internal set; }
        public Guid SewingInId { get; internal set; }
        public Guid SewingInItemId { get; internal set; }
        public int ProductId { get; internal set; }
        public string ProductCode { get; internal set; }
        public string ProductName { get; internal set; }
        public string DesignColor { get; internal set; }
        public int SizeId { get; internal set; }
        public string SizeName { get; internal set; }
        public double Quantity { get; internal set; }
        public double RemainingQuantity { get; internal set; }
        public int UomId { get; internal set; }
        public string UomUnit { get; internal set; }
        public string Color { get; internal set; }
        public string UId { get; private set; }
        public virtual GarmentServiceSubconSewingReadModel GarmentServiceSubconSewingIdentity { get; internal set; }
    }
}
