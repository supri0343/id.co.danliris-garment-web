using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.ServiceSampleCuttings.ReadModels
{
    public class GarmentServiceSampleCuttingSizeReadModel : ReadModelBase
    {
        public GarmentServiceSampleCuttingSizeReadModel(Guid identity) : base(identity)
        {
        }

        public Guid CuttingInId { get; internal set; }
        public Guid CuttingInDetailId { get; internal set; }
        public int ProductId { get; internal set; }
        public string ProductCode { get; internal set; }
        public string ProductName { get; internal set; }
        public int SizeId { get; internal set; }
        public string SizeName { get; internal set; }
        public double Quantity { get; internal set; }
        public int UomId { get; internal set; }
        public string UomUnit { get; internal set; }
        public string Color { get; internal set; }
        public Guid ServiceSampleCuttingDetailId { get; internal set; }

        public virtual GarmentServiceSampleCuttingDetailReadModel GarmentServiceSampleCuttingDetail { get; internal set; }
    }
}
