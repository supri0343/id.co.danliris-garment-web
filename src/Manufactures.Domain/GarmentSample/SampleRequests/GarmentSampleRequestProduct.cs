using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSample;
using Manufactures.Domain.GarmentSample.SampleRequests.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleRequests
{
    public class GarmentSampleRequestProduct : AggregateRoot<GarmentSampleRequestProduct, GarmentSampleRequestProductReadModel>
    {
        public Guid SampleRequestId { get; internal set; }
        public string Style { get; internal set; }
        public string Color { get; internal set; }

        public SizeId SizeId { get; internal set; }
        public string SizeName { get; internal set; }

        public string SizeDescription { get; internal set; }
        public double Quantity { get; internal set; }

        public GarmentSampleRequestProduct(Guid identity, Guid sampleRequestId, string style, string color, SizeId sizeId, string sizeName, string sizeDescription, double quantity) : base(identity)
        {
            Identity = identity;
            SampleRequestId = sampleRequestId;
            Style = style;
            Color = color;
            SizeId = sizeId;
            SizeName = sizeName;
            SizeDescription = sizeDescription;
            Quantity = quantity;

            ReadModel = new GarmentSampleRequestProductReadModel(Identity)
            {
                SampleRequestId=SampleRequestId,
                Style=Style,
                Color=Color,
                SizeId= SizeId.Value,
                SizeName=SizeName,
                SizeDescription=SizeDescription,
                Quantity=Quantity
            };

            ReadModel.AddDomainEvent(new OnGarmentSampleRequestPlaced(Identity));
        }

        public GarmentSampleRequestProduct(GarmentSampleRequestProductReadModel readModel) : base(readModel)
        {
            SampleRequestId = readModel.SampleRequestId;
            Style = readModel.Style;
            Color = readModel.Color;
            SizeId = new SizeId(readModel.SizeId);
            SizeName = readModel.SizeName;
            SizeDescription = readModel.SizeDescription;
            Quantity = readModel.Quantity;
        }


        protected override GarmentSampleRequestProduct GetEntity()
        {
            throw new NotImplementedException();
        }

        public void SetQuantity(double Quantity)
        {
            if (this.Quantity != Quantity)
            {
                this.Quantity = Quantity;
                ReadModel.Quantity = Quantity;
            }
        }

        public void Modify()
        {
            MarkModified();
        }
    }
}
