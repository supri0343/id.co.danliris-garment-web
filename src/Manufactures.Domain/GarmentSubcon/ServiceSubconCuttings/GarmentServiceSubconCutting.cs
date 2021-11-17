using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSubcon;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings
{
    public class GarmentServiceSubconCutting : AggregateRoot<GarmentServiceSubconCutting, GarmentServiceSubconCuttingReadModel>
    {

        public string SubconNo { get; private set; }
        public string SubconType { get; private set; }
        public UnitDepartmentId UnitId { get; private set; }
        public string UnitCode { get; private set; }
        public string UnitName { get; private set; }
        public DateTimeOffset SubconDate { get; private set; }

        public bool IsUsed { get; internal set; }
        public BuyerId BuyerId { get; private set; }
        public string BuyerCode { get; private set; }
        public string BuyerName { get; private set; }

        public GarmentServiceSubconCutting(Guid identity, string subconNo, string subconType, UnitDepartmentId unitId, string unitCode, string unitName, DateTimeOffset subconDate, bool isUsed, BuyerId buyerId, string buyerCode, string buyerName) : base(identity)
        {
            Identity = identity;
            SubconNo = subconNo;
            SubconType = subconType;
            UnitId = unitId;
            UnitCode = unitCode;
            UnitName = unitName;
            SubconDate = subconDate;
            IsUsed = isUsed;
            BuyerId = buyerId;
            BuyerCode = buyerCode;
            BuyerName = buyerName;

            ReadModel = new GarmentServiceSubconCuttingReadModel(Identity)
            {
                SubconDate = SubconDate,
                SubconNo = SubconNo,
                SubconType = SubconType,
                UnitCode = UnitCode,
                UnitId = UnitId.Value,
                UnitName = UnitName,
                IsUsed= isUsed,
                BuyerId = BuyerId.Value,
                BuyerCode = BuyerCode,
                BuyerName = BuyerName

            };

            ReadModel.AddDomainEvent(new OnServiceSubconCuttingPlaced(Identity));
        }

        public GarmentServiceSubconCutting(GarmentServiceSubconCuttingReadModel readModel) : base(readModel)
        {
            UnitName = readModel.UnitName;
            UnitId = new UnitDepartmentId(readModel.UnitId);
            UnitCode = readModel.UnitCode;
            SubconDate = readModel.SubconDate;
            SubconNo = readModel.SubconNo;
            SubconType = readModel.SubconType;
            IsUsed = readModel.IsUsed;
            IsUsed = readModel.IsUsed;
            BuyerId = new BuyerId(readModel.BuyerId);
            BuyerCode = readModel.BuyerCode;
            BuyerName = readModel.BuyerName;
        }

        public void SetDate(DateTimeOffset subconDate)
        {
            if (subconDate != SubconDate)
            {
                SubconDate = subconDate;
                ReadModel.SubconDate = subconDate;

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

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentServiceSubconCutting GetEntity()
        {
            return this;
        }
    }
}
