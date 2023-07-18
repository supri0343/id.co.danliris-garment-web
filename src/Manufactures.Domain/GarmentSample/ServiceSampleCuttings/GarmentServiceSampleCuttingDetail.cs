using Infrastructure.Domain;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.Events.GarmentSample;

namespace Manufactures.Domain.GarmentSample.ServiceSampleCuttings
{
    public class GarmentServiceSampleCuttingDetail : AggregateRoot<GarmentServiceSampleCuttingDetail, GarmentServiceSampleCuttingDetailReadModel>
    {

        public Guid ServiceSampleCuttingItemId { get; private set; }

        public string DesignColor { get; private set; }
        public double Quantity { get; private set; }

        public GarmentServiceSampleCuttingDetail(Guid identity, Guid serviceSampleCuttingItemId,  string designColor, double quantity) : base(identity)
        {
            Identity = identity;
            ServiceSampleCuttingItemId = serviceSampleCuttingItemId;
            DesignColor = designColor;
            Quantity = quantity;

            ReadModel = new GarmentServiceSampleCuttingDetailReadModel(Identity)
            {
                ServiceSampleCuttingItemId = ServiceSampleCuttingItemId,
                DesignColor = DesignColor,
                Quantity = Quantity
            };

            ReadModel.AddDomainEvent(new OnServiceSampleCuttingPlaced(Identity));
        }

        public GarmentServiceSampleCuttingDetail(GarmentServiceSampleCuttingDetailReadModel readModel) : base(readModel)
        {
            ServiceSampleCuttingItemId = readModel.ServiceSampleCuttingItemId;
            DesignColor = readModel.DesignColor;
            Quantity = readModel.Quantity;
        }

        public void SetQuantity(double Quantity)
        {
            if (this.Quantity != Quantity)
            {
                this.Quantity = Quantity;
                ReadModel.Quantity = Quantity;
            }
        }

        public void SetDesignColor(string DesignColor)
        {
            if (this.DesignColor != DesignColor)
            {
                this.DesignColor = DesignColor;
                ReadModel.DesignColor = DesignColor;
            }
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentServiceSampleCuttingDetail GetEntity()
        {
            return this;
        }
    }
}
