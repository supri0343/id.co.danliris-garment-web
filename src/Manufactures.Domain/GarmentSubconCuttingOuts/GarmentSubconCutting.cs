using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentSubconCuttingOuts.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubconCuttingOuts
{
    public class GarmentSubconCutting : AggregateRoot<GarmentSubconCutting, GarmentSubconCuttingReadModel>
    {
        public string RONo { get; private set; }
        public SizeId SizeId { get; private set; }
        public string SizeName { get; private set; }
        public double Quantity { get; private set; }

        public void SetQuantity(double Quantity)
        {
            if (this.Quantity != Quantity)
            {
                this.Quantity = Quantity;
                ReadModel.Quantity = Quantity;
            }
        }

        public GarmentSubconCutting(Guid identity, string roNo, SizeId sizeId, string sizeName, double quantity) : base(identity)
        {
            RONo = roNo;
            SizeId = sizeId;
            SizeName = sizeName;
            Quantity = quantity;

            ReadModel = new GarmentSubconCuttingReadModel(Identity)
            {
                RONo=RONo,
                SizeId = SizeId.Value,
                SizeName = SizeName,
                Quantity = Quantity
            };

            ReadModel.AddDomainEvent(new OnGarmentSubconCuttingPlaced(Identity));
        }

        public GarmentSubconCutting(GarmentSubconCuttingReadModel readModel) : base(readModel)
        {
            RONo = readModel.RONo;
            SizeId = new SizeId(readModel.SizeId);
            SizeName = readModel.SizeName;
            Quantity = readModel.Quantity;
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentSubconCutting GetEntity()
        {
            return this;
        }
    }
}
