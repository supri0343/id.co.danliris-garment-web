using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSubcon;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings
{
    public class GarmentServiceSubconCuttingSize : AggregateRoot<GarmentServiceSubconCuttingSize, GarmentServiceSubconCuttingSizeReadModel>
    {

        public SizeId SizeId { get; internal set; }
        public string SizeName { get; internal set; }
        public double Quantity { get; internal set; }
        public UomId UomId { get; internal set; }
        public string UomUnit { get; internal set; }
        public string Color { get; internal set; }
        public Guid ServiceSubconCuttingDetailId { get; internal set; }
        public GarmentServiceSubconCuttingSize(Guid identity,SizeId sizeId, string sizeName, double quantity, UomId uomId, string uomUnit, string color, Guid serviceSubconCuttingDetailId) : base(identity)
        {
            Identity = identity;
            SizeId = sizeId;
            SizeName = sizeName;
            Quantity = quantity;
            UomId = uomId;
            UomUnit = uomUnit;
            Color = color;
            ServiceSubconCuttingDetailId = serviceSubconCuttingDetailId;
            ReadModel = new GarmentServiceSubconCuttingSizeReadModel(Identity)
            {
                ServiceSubconCuttingDetailId = ServiceSubconCuttingDetailId,
                Color = Color,
                SizeId = SizeId.Value,
                SizeName = SizeName,
                Quantity = Quantity,
                UomId = UomId.Value,
                UomUnit = UomUnit
            };

            ReadModel.AddDomainEvent(new OnServiceSubconCuttingPlaced(Identity));
        }

        public GarmentServiceSubconCuttingSize(GarmentServiceSubconCuttingSizeReadModel readModel) : base(readModel)
        {
            ServiceSubconCuttingDetailId = readModel.ServiceSubconCuttingDetailId;
            Color = readModel.Color;
            SizeId = new SizeId(readModel.SizeId);
            SizeName = readModel.SizeName;
            Quantity = readModel.Quantity;
            UomId = new UomId(readModel.UomId);
            UomUnit = readModel.UomUnit;
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentServiceSubconCuttingSize GetEntity()
        {
            return this;
        }
    }
}
