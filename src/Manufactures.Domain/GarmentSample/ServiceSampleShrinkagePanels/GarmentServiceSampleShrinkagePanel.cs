using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSample;
using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels
{
    public class GarmentServiceSampleShrinkagePanel : AggregateRoot<GarmentServiceSampleShrinkagePanel, GarmentServiceSampleShrinkagePanelReadModel>
    {
        public string ServiceSampleShrinkagePanelNo { get; private set; }
        public DateTimeOffset ServiceSampleShrinkagePanelDate { get; private set; }
        public string Remark { get; private set; }
        public bool IsUsed { get; private set; }
        public int QtyPacking { get; private set; }
        public string UomUnit { get; private set; }
        public double NettWeight { get; private set; }
        public double GrossWeight { get; private set; }
        public GarmentServiceSampleShrinkagePanel(Guid identity, string serviceSampleShrinkagePanelNo, DateTimeOffset serviceSampleShrinkagePanelDate, string remark, bool isUsed, int qtyPacking, string uomUnit, double nettWeight, double grossWeight) : base(identity)
        {
            Identity = identity;
            ServiceSampleShrinkagePanelNo = serviceSampleShrinkagePanelNo;
            ServiceSampleShrinkagePanelDate = serviceSampleShrinkagePanelDate;
            Remark = remark;
            IsUsed = isUsed;
            QtyPacking = qtyPacking;
            UomUnit = uomUnit;
            NettWeight = nettWeight;
            GrossWeight = grossWeight;

            ReadModel = new GarmentServiceSampleShrinkagePanelReadModel(Identity)
            {
                ServiceSampleShrinkagePanelNo = ServiceSampleShrinkagePanelNo,
                ServiceSampleShrinkagePanelDate = ServiceSampleShrinkagePanelDate,
                Remark = Remark,
                IsUsed = IsUsed,
                QtyPacking = QtyPacking,
                UomUnit = UomUnit,
                NettWeight = NettWeight,
                GrossWeight = GrossWeight
            };

            ReadModel.AddDomainEvent(new OnGarmentServiceSampleShrinkagePanelPlaced(Identity));
        }

        public GarmentServiceSampleShrinkagePanel(GarmentServiceSampleShrinkagePanelReadModel readModel) : base(readModel)
        {
            ServiceSampleShrinkagePanelNo = readModel.ServiceSampleShrinkagePanelNo;
            ServiceSampleShrinkagePanelDate = readModel.ServiceSampleShrinkagePanelDate;
            Remark = readModel.Remark;
            IsUsed = readModel.IsUsed;
            QtyPacking = readModel.QtyPacking;
            UomUnit = readModel.UomUnit;
            NettWeight = readModel.NettWeight;
            GrossWeight = readModel.GrossWeight;
        }

        public void SetServiceSampleShrinkagePanelDate(DateTimeOffset ServiceSampleShrinkagePanelDate)
        {
            if (this.ServiceSampleShrinkagePanelDate != ServiceSampleShrinkagePanelDate)
            {
                this.ServiceSampleShrinkagePanelDate = ServiceSampleShrinkagePanelDate;
                ReadModel.ServiceSampleShrinkagePanelDate = ServiceSampleShrinkagePanelDate;
            }
        }

        public void SetRemark(string remark)
        {
            if (remark != Remark)
            {
                Remark = remark;
                ReadModel.Remark = remark;

                MarkModified();
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

        protected override GarmentServiceSampleShrinkagePanel GetEntity()
        {
            return this;
        }
    }
}
