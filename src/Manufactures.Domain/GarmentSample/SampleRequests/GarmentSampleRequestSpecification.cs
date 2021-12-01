using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSample;
using Manufactures.Domain.GarmentSample.SampleRequests.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleRequests
{
    public class GarmentSampleRequestSpecification : AggregateRoot<GarmentSampleRequestSpecification, GarmentSampleRequestSpecificationReadModel>
    {
        public Guid SampleRequestId { get; internal set; }
        public string Inventory { get; internal set; }
        public string SpecificationDetail { get; internal set; }
        public double Quantity { get; internal set; }
        public string Remark { get; internal set; }

        public GarmentSampleRequestSpecification(Guid identity, Guid sampleRequestId, string inventory, string specificationDetail, double quantity, string remark) : base(identity)
        {
            Identity = identity;
            SampleRequestId = sampleRequestId;
            Inventory = inventory;
            SpecificationDetail = specificationDetail;
            Quantity = quantity;
            Remark = remark;

            ReadModel = new GarmentSampleRequestSpecificationReadModel(Identity)
            {
                SampleRequestId=SampleRequestId,
                Inventory=Inventory,
                SpecificationDetail=SpecificationDetail,
                Quantity=Quantity,
                Remark=Remark
            };
            ReadModel.AddDomainEvent(new OnGarmentSampleRequestPlaced(Identity));
        }

        public GarmentSampleRequestSpecification(GarmentSampleRequestSpecificationReadModel readModel) : base(readModel)
        {
            SampleRequestId = readModel.SampleRequestId;
            Inventory = readModel.Inventory;
            SpecificationDetail = readModel.SpecificationDetail;
            Quantity = readModel.Quantity;
            Remark = readModel.Remark;
        }

        protected override GarmentSampleRequestSpecification GetEntity()
        {
            return this;
        }

        public void SetQuantity(double Quantity)
        {
            if (this.Quantity != Quantity)
            {
                this.Quantity = Quantity;
                ReadModel.Quantity = Quantity;
            }
        }

        public void SetRemark(string remark)
        {
            if (this.Remark != remark)
            {
                this.Remark = remark;
                ReadModel.Remark = remark;
            }
        }
        public void SetInventory(string Inventory)
        {
            if (this.Inventory != Inventory)
            {
                this.Inventory = Inventory;
                ReadModel.Inventory = Inventory;
            }
        }
        public void SetSpecificationDetail(string SpecificationDetail)
        {
            if (this.SpecificationDetail != SpecificationDetail)
            {
                this.SpecificationDetail = SpecificationDetail;
                ReadModel.SpecificationDetail = SpecificationDetail;
            }
        }

        public void Modify()
        {
            MarkModified();
        }
    }
}
