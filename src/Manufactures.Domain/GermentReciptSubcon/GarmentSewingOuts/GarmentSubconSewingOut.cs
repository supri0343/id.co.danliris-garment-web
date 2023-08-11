using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentSewingOuts.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingOuts.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentSewingOuts
{
    public class GarmentSubconSewingOut : AggregateRoot<GarmentSubconSewingOut, GarmentSubconSewingOutReadModel>
    {
        public string SewingOutNo { get; private set; }
        public BuyerId ProductOwnerId { get; private set; }
        public string ProductOwnerCode { get; private set; }
        public string ProductOwnerName { get; private set; }
        public UnitDepartmentId UnitId { get; private set; }
        public string UnitCode { get; private set; }
        public string UnitName { get; private set; }
        public string SewingTo { get; private set; }
        public UnitDepartmentId UnitToId { get; private set; }
        public string UnitToCode { get; private set; }
        public string UnitToName { get; private set; }
        public string RONo { get; private set; }
        public string Article { get; private set; }
        public GarmentComodityId ComodityId { get; private set; }
        public string ComodityCode { get; private set; }
        public string ComodityName { get; private set; }
        public DateTimeOffset SewingOutDate { get; private set; }
        public bool IsDifferentSize { get; private set; }


        public GarmentSubconSewingOut(Guid identity, string sewingOutNo, BuyerId buyerId, string buyerCode, string buyerName, UnitDepartmentId unitToId, string unitToCode, string unitToName, string sewingTo, DateTimeOffset sewingOutDate, string rONo, string article, UnitDepartmentId unitId, string unitCode, string unitName, GarmentComodityId comodityId, string comodityCode, string comodityName, bool isDifferentSize) : base(identity)
        {
            Validator.ThrowIfNull(() => unitId);
            Validator.ThrowIfNull(() => unitToId);
            Validator.ThrowIfNull(() => rONo);

            //MarkTransient();

            Identity = identity;
            SewingOutNo = sewingOutNo;
            SewingTo = sewingTo;
            UnitToId = unitToId;
            UnitToCode = unitToCode;
            UnitToName = unitToName;
            SewingOutDate = sewingOutDate;
            RONo = rONo;
            Article = article;
            UnitId = unitId;
            UnitCode = unitCode;
            UnitName = unitName;
            ComodityId = comodityId;
            ComodityCode = comodityCode;
            ComodityName = comodityName;
            ProductOwnerCode = buyerCode;
            ProductOwnerId = buyerId;
            ProductOwnerName = buyerName;
            IsDifferentSize = isDifferentSize;

            ReadModel = new GarmentSubconSewingOutReadModel(Identity)
            {
                SewingOutNo = SewingOutNo,
                SewingTo = SewingTo,
                UnitToId = UnitToId.Value,
                UnitToCode = UnitToCode,
                UnitToName = UnitToName,
                SewingOutDate = SewingOutDate,
                RONo = RONo,
                Article = Article,
                UnitId = UnitId.Value,
                UnitCode = UnitCode,
                UnitName = UnitName,
                ComodityId = ComodityId.Value,
                ComodityCode = ComodityCode,
                ComodityName = ComodityName,
                ProductOwnerCode = ProductOwnerCode,
                ProductOwnerId = ProductOwnerId.Value,
                ProductOwnerName = ProductOwnerName,
                IsDifferentSize=IsDifferentSize,
            };

            ReadModel.AddDomainEvent(new OnGarmentSewingOutPlaced(Identity));
        }

        public GarmentSubconSewingOut(GarmentSubconSewingOutReadModel readModel) : base(readModel)
        {
            SewingOutNo = readModel.SewingOutNo;
            SewingTo = readModel.SewingTo;
            UnitToId = new UnitDepartmentId(readModel.UnitToId);
            UnitToCode = readModel.UnitToCode;
            UnitToName = readModel.UnitToName;
            SewingOutDate = readModel.SewingOutDate;
            RONo = readModel.RONo;
            Article = readModel.Article;
            UnitId = new UnitDepartmentId(readModel.UnitId);
            UnitCode = readModel.UnitCode;
            UnitName = readModel.UnitName;
            ComodityId = new GarmentComodityId(readModel.ComodityId);
            ComodityCode = readModel.ComodityCode;
            ComodityName = readModel.ComodityName;
            ProductOwnerCode = readModel.ProductOwnerCode;
            ProductOwnerId = new BuyerId(readModel.ProductOwnerId);
            ProductOwnerName = readModel.ProductOwnerName;
            IsDifferentSize = readModel.IsDifferentSize;
        }
        public void SetDate(DateTimeOffset SewingOutDate)
        {
            if (this.SewingOutDate != SewingOutDate)
            {
                this.SewingOutDate = SewingOutDate;
                ReadModel.SewingOutDate = SewingOutDate;
            }
        }
        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentSubconSewingOut GetEntity()
        {
            return this;
        }
    }
}
