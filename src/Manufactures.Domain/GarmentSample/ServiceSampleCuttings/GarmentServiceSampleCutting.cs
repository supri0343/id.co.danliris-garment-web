using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSample;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.ServiceSampleCuttings
{
    public class GarmentServiceSampleCutting : AggregateRoot<GarmentServiceSampleCutting, GarmentServiceSampleCuttingReadModel>
    {

        public string SampleNo { get; private set; }
        public string SampleType { get; private set; }
        public UnitDepartmentId UnitId { get; private set; }
        public string UnitCode { get; private set; }
        public string UnitName { get; private set; }
        public DateTimeOffset SampleDate { get; private set; }
        public bool IsUsed { get; internal set; }
        public BuyerId BuyerId { get; private set; }
        public string BuyerCode { get; private set; }
        public string BuyerName { get; private set; }
        public UomId UomId { get; private set; }
        public string UomUnit { get; private set; }
        public int QtyPacking { get; private set; }
        public double NettWeight { get; private set; }
        public double GrossWeight { get; private set; }
        public string Remark { get; private set; }

        public GarmentServiceSampleCutting(Guid identity, string SampleNo, string SampleType, UnitDepartmentId unitId, string unitCode, string unitName, DateTimeOffset SampleDate, bool isUsed, BuyerId buyerId, string buyerCode, string buyerName, UomId uomId, string uomUnit, int qtyPacking, double nettWeight, double grossWeight, string remark) : base(identity)
        {
            Identity = identity;
            SampleNo = SampleNo;
            SampleType = SampleType;
            UnitId = unitId;
            UnitCode = unitCode;
            UnitName = unitName;
            SampleDate = SampleDate;
            IsUsed = isUsed;
            BuyerId = buyerId;
            BuyerCode = buyerCode;
            BuyerName = buyerName;
            UomId = uomId;
            UomUnit = uomUnit;
            QtyPacking = qtyPacking;
            NettWeight = nettWeight;
            GrossWeight = grossWeight;
            Remark = remark;

            ReadModel = new GarmentServiceSampleCuttingReadModel(Identity)
            {
                SampleDate = SampleDate,
                SampleNo = SampleNo,
                SampleType = SampleType,
                UnitCode = UnitCode,
                UnitId = UnitId.Value,
                UnitName = UnitName,
                IsUsed = isUsed,
                BuyerId = BuyerId.Value,
                BuyerCode = BuyerCode,
                BuyerName = BuyerName,
                UomId = UomId.Value,
                UomUnit = UomUnit,
                QtyPacking = QtyPacking,
                NettWeight = NettWeight,
                GrossWeight = GrossWeight,
                Remark = Remark,
            };

            ReadModel.AddDomainEvent(new OnServiceSampleCuttingPlaced(Identity));
        }

        public GarmentServiceSampleCutting(GarmentServiceSampleCuttingReadModel readModel) : base(readModel)
        {
            UnitName = readModel.UnitName;
            UnitId = new UnitDepartmentId(readModel.UnitId);
            UnitCode = readModel.UnitCode;
            SampleDate = readModel.SampleDate;
            SampleNo = readModel.SampleNo;
            SampleType = readModel.SampleType;
            IsUsed = readModel.IsUsed;
            IsUsed = readModel.IsUsed;
            BuyerId = new BuyerId(readModel.BuyerId);
            BuyerCode = readModel.BuyerCode;
            BuyerName = readModel.BuyerName;
            UomId = new UomId(readModel.UomId);
            UomUnit = readModel.UomUnit;
            QtyPacking = readModel.QtyPacking;
            NettWeight = readModel.NettWeight;
            GrossWeight = readModel.GrossWeight;
            Remark = readModel.Remark;
        }

        public void SetDate(DateTimeOffset SampleDate)
        {
            if (SampleDate != SampleDate)
            {
                SampleDate = SampleDate;
                ReadModel.SampleDate = SampleDate;

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
        public void SetBuyerId(BuyerId SupplierId)
        {
            if (this.BuyerId != BuyerId)
            {
                this.BuyerId = BuyerId;
                ReadModel.BuyerId = BuyerId.Value;
            }
        }
        public void SetBuyerCode(string BuyerCode)
        {
            if (this.BuyerCode != BuyerCode)
            {
                this.BuyerCode = BuyerCode;
                ReadModel.BuyerCode = BuyerCode;
            }
        }
        public void SetBuyerName(string BuyerName)
        {
            if (this.BuyerName != BuyerName)
            {
                this.BuyerName = BuyerName;
                ReadModel.BuyerName = BuyerName;
            }
        }

        public void SetUomId (UomId UomId)
        {
            if(this.UomId != UomId)
            {
                this.UomId = UomId;
                ReadModel.UomId = UomId.Value;
            }
        }

        public void SetUomUnit(string UomUnit)
        {
            if(this.UomUnit != UomUnit)
            {
                this.UomUnit = UomUnit;
                ReadModel.UomUnit = UomUnit;
            }
        }

        public void SetQtyPacking (int QtyPacking)
        {
            if(this.QtyPacking != QtyPacking)
            {
                this.QtyPacking = QtyPacking;
                ReadModel.QtyPacking = QtyPacking;
            }
        }

        public void SetNettWeight (double NettWeight)
        {
            if(this.NettWeight != NettWeight)
            {
                this.NettWeight = NettWeight;
                ReadModel.NettWeight = NettWeight;
            }
        }

        public void SetGrossWeight (double GrossWeight)
        {
            if(this.GrossWeight != GrossWeight)
            {
                this.GrossWeight = GrossWeight;
                ReadModel.GrossWeight = GrossWeight;
            }
        }

        public void SetRemark (string Remark)
        {
            if(this.Remark != Remark)
            {
                this.Remark = Remark;
                ReadModel.Remark = Remark;
            }
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentServiceSampleCutting GetEntity()
        {
            return this;
        }
    }
}
