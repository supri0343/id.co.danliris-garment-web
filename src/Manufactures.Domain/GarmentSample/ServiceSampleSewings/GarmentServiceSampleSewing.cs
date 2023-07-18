using System;
using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSample;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using Moonlay;

namespace Manufactures.Domain.GarmentSample.ServiceSampleSewings
{
    public class GarmentServiceSampleSewing : AggregateRoot<GarmentServiceSampleSewing, GarmentServiceSampleSewingReadModel>
    {
        public string ServiceSampleSewingNo { get; private set; }
        public DateTimeOffset ServiceSampleSewingDate { get; private set; }
        public bool IsUsed { get; internal set; }
        public BuyerId BuyerId { get; private set; }
        public string BuyerCode { get; private set; }
        public string BuyerName { get; private set; }
        public int QtyPacking { get; private set; }
        public string UomUnit { get; private set; }
        public double NettWeight { get; private set; }
        public double GrossWeight { get; private set; }
        //
        public GarmentServiceSampleSewing(Guid identity, string serviceSampleSewingNo, DateTimeOffset serviceSampleSewingDate, bool isUsed, BuyerId buyerId, string buyerCode, string buyerName, int qtyPacking, string uomUnit, double nettWeight, double grossWeight) : base(identity)
        {
            
            Identity = identity;
            ServiceSampleSewingNo = serviceSampleSewingNo;
            ServiceSampleSewingDate = serviceSampleSewingDate;
            IsUsed = isUsed;
            BuyerId = buyerId;
            BuyerCode = buyerCode;
            BuyerName = buyerName;
            QtyPacking = qtyPacking;
            UomUnit = uomUnit;
            NettWeight = nettWeight;
            GrossWeight = grossWeight; 

            ReadModel = new GarmentServiceSampleSewingReadModel(Identity)
            {
                ServiceSampleSewingNo = ServiceSampleSewingNo,
                ServiceSampleSewingDate = ServiceSampleSewingDate,
                //UnitId = UnitId.Value,
                //UnitCode = UnitCode,
                //UnitName = UnitName,
                IsUsed = IsUsed,
                BuyerId = BuyerId.Value,
                BuyerCode = BuyerCode,
                BuyerName = BuyerName,
                QtyPacking = QtyPacking,
                UomUnit = UomUnit,
                NettWeight = NettWeight,
                GrossWeight = GrossWeight
            };

            ReadModel.AddDomainEvent(new OnGarmentServiceSampleSewingPlaced(Identity));

        }

        public GarmentServiceSampleSewing(GarmentServiceSampleSewingReadModel readModel) : base(readModel)
        {
            ServiceSampleSewingNo = readModel.ServiceSampleSewingNo;
            ServiceSampleSewingDate = readModel.ServiceSampleSewingDate;
            //UnitId = new UnitDepartmentId(readModel.UnitId);
            //UnitCode = readModel.UnitCode;
            //UnitName = readModel.UnitName;
            IsUsed = readModel.IsUsed;
            BuyerId = new BuyerId(readModel.BuyerId);
            BuyerCode = readModel.BuyerCode;
            BuyerName = readModel.BuyerName;
            QtyPacking = readModel.QtyPacking;
            UomUnit = readModel.UomUnit;
            NettWeight = readModel.NettWeight;
            GrossWeight = readModel.GrossWeight;
        }

        public void SetDate(DateTimeOffset ServiceSampleSewingDate)
        {
            if (this.ServiceSampleSewingDate != ServiceSampleSewingDate)
            {
                this.ServiceSampleSewingDate = ServiceSampleSewingDate;
                ReadModel.ServiceSampleSewingDate = ServiceSampleSewingDate;
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
            if(this.UomUnit != uomUnit)
            {
                this.UomUnit = uomUnit;
                ReadModel.UomUnit = uomUnit;
            }
        }

        public void SetNettWeight(double nettWeight)
        {
            if(this.NettWeight != nettWeight)
            {
                this.NettWeight = nettWeight;
                ReadModel.NettWeight = nettWeight;
            }
        }

        public void SetGrossWeight(double grossWeight)
        {
            if(this.GrossWeight != grossWeight)
            {
                this.GrossWeight = grossWeight;
                ReadModel.GrossWeight = grossWeight;
            }
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentServiceSampleSewing GetEntity()
        {
            return this;
        }
    }
}
