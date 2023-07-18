using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSample;
using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes
{
    public class GarmentServiceSampleFabricWash : AggregateRoot<GarmentServiceSampleFabricWash, GarmentServiceSampleFabricWashReadModel>
    {
        public string ServiceSampleFabricWashNo { get; private set; }
        public DateTimeOffset ServiceSampleFabricWashDate { get; private set; }
        public string Remark { get; private set; }
        public bool IsUsed { get; private set; }
        public int QtyPacking { get; private set; }
        public string UomUnit { get; private set; }
        public double NettWeight { get; private set; }
        public double GrossWeight { get; private set; }

        public GarmentServiceSampleFabricWash(Guid identity, string serviceSampleFabricWashNo, DateTimeOffset serviceSampleFabricWashDate, string remark, bool isUsed, int qtyPacking, string uomUnit, double nettWeight, double grossWeight) : base(identity)
        {
            Identity = identity;
            ServiceSampleFabricWashNo = serviceSampleFabricWashNo;
            ServiceSampleFabricWashDate = serviceSampleFabricWashDate;
            IsUsed = isUsed;
            Remark = remark;
            QtyPacking = qtyPacking;
            UomUnit = uomUnit;
            NettWeight = nettWeight;
            GrossWeight = grossWeight;

            ReadModel = new GarmentServiceSampleFabricWashReadModel(Identity)
            {
                ServiceSampleFabricWashNo = ServiceSampleFabricWashNo,
                ServiceSampleFabricWashDate = ServiceSampleFabricWashDate,
                Remark = Remark,
                IsUsed = IsUsed,
                QtyPacking = QtyPacking,
                UomUnit = UomUnit,
                NettWeight = NettWeight,
                GrossWeight = GrossWeight,
            };

            ReadModel.AddDomainEvent(new OnGarmentServiceSampleFabricWashPlaced(Identity));
        }

        public GarmentServiceSampleFabricWash(GarmentServiceSampleFabricWashReadModel readModel) : base(readModel)
        {
            ServiceSampleFabricWashNo = readModel.ServiceSampleFabricWashNo;
            ServiceSampleFabricWashDate = readModel.ServiceSampleFabricWashDate;
            Remark = readModel.Remark;
            IsUsed = readModel.IsUsed;
            QtyPacking = readModel.QtyPacking;
            UomUnit = readModel.UomUnit;
            NettWeight = readModel.NettWeight;
            GrossWeight = readModel.GrossWeight;
        }

        public void SetServiceSampleFabricWashDate(DateTimeOffset ServiceSampleFabricWashDate)
        {
            if (this.ServiceSampleFabricWashDate != ServiceSampleFabricWashDate)
            {
                this.ServiceSampleFabricWashDate = ServiceSampleFabricWashDate;
                ReadModel.ServiceSampleFabricWashDate = ServiceSampleFabricWashDate;
            }
        }

        public void SetIsUsed(bool isUsed)
        {
            if (isUsed != IsUsed)
            {
                IsUsed = isUsed;
                ReadModel.IsUsed = isUsed;

                MarkModified();
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

        public void SetQtyPacking(int qtyPacking)
        {
            if (this.QtyPacking != qtyPacking)
            {
                this.QtyPacking = qtyPacking;
                ReadModel.QtyPacking = qtyPacking;
            }
        }

        public void SetUomUnit(string uomUnit)
        {
            if (this.UomUnit != uomUnit)
            {
                this.UomUnit = uomUnit;
                ReadModel.UomUnit = UomUnit;
            }
        }

        public void SetNettWeight(double nettWeight)
        {
            if(this.NettWeight != nettWeight)
            {
                this.NettWeight = nettWeight;
                ReadModel.NettWeight = NettWeight;
            }
        }

        public void SetGrossWeight(double grossWeight)
        {
            if(this.GrossWeight != grossWeight)
            {
                this.GrossWeight = grossWeight;
                ReadModel.GrossWeight = GrossWeight;
            }
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentServiceSampleFabricWash GetEntity()
        {
            return this;
        }
    }
}
