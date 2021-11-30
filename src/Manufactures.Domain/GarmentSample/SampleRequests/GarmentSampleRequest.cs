using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSample;
using Manufactures.Domain.GarmentSample.SampleRequests.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleRequests
{
    public class GarmentSampleRequest : AggregateRoot<GarmentSampleRequest, GarmentSampleRequestReadModel>
    {
        public string SampleCategory { get; private set; }
        public string SampleRequestNo { get; private set; }
        public string RONoSample { get; internal set; }
        public string RONoCC { get; internal set; }
        public DateTimeOffset Date { get; private set; }

        public BuyerId BuyerId { get; private set; }
        public string BuyerCode { get; private set; }
        public string BuyerName { get; private set; }

        public GarmentComodityId ComodityId { get; private set; }
        public string ComodityCode { get; private set; }
        public string ComodityName { get; private set; }

        public string SampleType { get; private set; }
        public string Packing { get; private set; }
        public DateTimeOffset SentDate { get; private set; }
        public string POBuyer { get; private set; }
        public string Attached { get; private set; }
        public string Remark { get; private set; }
        public bool IsPosted { get; private set; }
        public bool IsReceived { get; private set; }

        public GarmentSampleRequest(Guid identity, string sampleCategory, string sampleRequestNo, string rONoSample, string rONoCC, DateTimeOffset date, BuyerId buyerId, string buyerCode, string buyerName, GarmentComodityId comodityId, string comodityCode, string comodityName, string sampleType, string packing, DateTimeOffset sentDate, string pOBuyer, string attached, string remark, bool isPosted, bool isReceived) : base(identity)
        {
            Identity = identity;
            SampleCategory = sampleCategory;
            SampleRequestNo = sampleRequestNo;
            RONoSample = rONoSample;
            RONoCC = rONoCC;
            Date = date;
            BuyerId = buyerId;
            BuyerCode = buyerCode;
            BuyerName = buyerName;
            ComodityId = comodityId;
            ComodityCode = comodityCode;
            ComodityName = comodityName;
            SampleType = sampleType;
            Packing = packing;
            SentDate = sentDate;
            POBuyer = pOBuyer;
            Attached = attached;
            Remark = remark;
            IsPosted = isPosted;
            IsReceived = isReceived;

            ReadModel = new GarmentSampleRequestReadModel(Identity)
            {
                SampleCategory = SampleCategory,
                SampleRequestNo = SampleRequestNo,
                RONoSample = RONoSample,
                RONoCC= RONoCC,
                Date = Date,
                BuyerId = BuyerId.Value,
                BuyerCode = BuyerCode,
                BuyerName = BuyerName,
                ComodityId = ComodityId.Value,
                ComodityCode = ComodityCode,
                ComodityName = ComodityName,
                SampleType = SampleType,
                Packing = Packing,
                SentDate = SentDate,
                POBuyer = POBuyer,
                Attached = Attached,
                Remark = Remark,
                IsPosted = IsPosted,
                IsReceived = IsReceived

            };
            ReadModel.AddDomainEvent(new OnGarmentSampleRequestPlaced(Identity));
        }

        public GarmentSampleRequest(GarmentSampleRequestReadModel readModel) : base(readModel)
        {
            SampleCategory = readModel.SampleCategory;
            SampleRequestNo = readModel.SampleRequestNo;
            RONoSample = readModel.RONoSample;
            RONoCC = readModel.RONoCC;
            Date = readModel.Date;
            BuyerId = new BuyerId(readModel.BuyerId);
            BuyerCode = readModel.BuyerCode;
            BuyerName = readModel.BuyerName;
            ComodityId = new GarmentComodityId(readModel.ComodityId);
            ComodityCode = readModel.ComodityCode;
            ComodityName = readModel.ComodityName;
            SampleType = readModel.SampleType;
            Packing = readModel.Packing;
            SentDate = readModel.SentDate;
            POBuyer = readModel.POBuyer;
            Attached = readModel.Attached;
            Remark = readModel.Remark;
            IsPosted = readModel.IsPosted;
            IsReceived = readModel.IsReceived;
        }

        protected override GarmentSampleRequest GetEntity()
        {
            throw new NotImplementedException();
        }

        public void SetIsPosted(bool IsPosted)
        {
            if (this.IsPosted != IsPosted)
            {
                this.IsPosted = IsPosted;
                ReadModel.IsPosted = IsPosted;
            }
        }

        public void SetIsReceived(bool IsReceived)
        {
            if (this.IsReceived != IsReceived)
            {
                this.IsReceived = IsReceived;
                ReadModel.IsReceived = IsReceived;
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

        public void SetRemark(string remark)
        {
            if (this.Remark != remark)
            {
                this.Remark = remark;
                ReadModel.Remark = remark;
            }
        }

        public void Modify()
        {
            MarkModified();
        }
    }
}
