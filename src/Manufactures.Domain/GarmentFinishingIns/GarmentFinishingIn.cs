using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentFinishingIns.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentFinishingIns
{
    public class GarmentFinishingIn : AggregateRoot<GarmentFinishingIn, GarmentFinishingInReadModel>
    {
        public string FinishingInNo { get; internal set; }
        public string FinishingInType { get; internal set; }
        public UnitDepartmentId UnitId { get; internal set; }
        public string UnitCode { get; internal set; }
        public string UnitName { get; internal set; }
        public Guid SewingOutId { get; internal set; }
        public string SewingOutNo { get; internal set; }
        public UnitDepartmentId UnitFromId { get; internal set; }
        public string UnitFromCode { get; internal set; }
        public string UnitFromName { get; internal set; }
        public string Article { get; internal set; }
        public string RONo { get; internal set; }
        public GarmentComodityId ComodityId { get; internal set; }
        public string ComodityCode { get; internal set; }
        public string ComodityName { get; internal set; }
        public DateTimeOffset FinishingInDate { get; internal set; }

        public GarmentFinishingIn(Guid identity, string finishingInNo, Guid sewingOutId, string sewingOutNo, string finishingInType, UnitDepartmentId unitFromId, string unitFromCode, string unitFromName, string rONo, string article, UnitDepartmentId unitId, string unitCode, string unitName, DateTimeOffset finishingInDate, GarmentComodityId comodityId, string comodityCode, string comodityName) : base(identity)
        {
            Validator.ThrowIfNull(() => unitId);
            Validator.ThrowIfNull(() => sewingOutId);

            //MarkTransient();
            FinishingInNo = finishingInNo;
            Identity = identity;
            FinishingInType = finishingInType;
            SewingOutId = sewingOutId;
            SewingOutNo = sewingOutNo;
            UnitFromCode = unitFromCode;
            UnitFromName = unitFromName;
            UnitFromId = unitFromId;
            RONo = rONo;
            Article = article;
            UnitId = unitId;
            UnitCode = unitCode;
            UnitName = unitName;
            ComodityId = comodityId;
            FinishingInDate = finishingInDate;
            ComodityCode = comodityCode;
            ComodityName = comodityName;

            ReadModel = new GarmentFinishingInReadModel(Identity)
            {
                FinishingInDate = FinishingInDate,
                FinishingInNo = FinishingInNo,
                FinishingInType= FinishingInType,
                RONo = RONo,
                Article = Article,
                UnitId = UnitId.Value,
                UnitCode = UnitCode,
                UnitName = UnitName,
                SewingOutId = SewingOutId,
                SewingOutNo = SewingOutNo,
                UnitFromCode = UnitFromCode,
                UnitFromName = UnitFromName,
                UnitFromId = UnitFromId.Value,
                ComodityId = ComodityId.Value,
                ComodityCode = ComodityCode,
                ComodityName = ComodityName,

            };

            ReadModel.AddDomainEvent(new OnGarmentFinishingInPlaced(Identity));
        }

        public GarmentFinishingIn(GarmentFinishingInReadModel readModel) : base(readModel)
        {
            FinishingInNo = readModel.FinishingInNo;
            RONo = readModel.RONo;
            Article = readModel.Article;
            UnitId = new UnitDepartmentId(readModel.UnitId);
            UnitCode = readModel.UnitCode;
            UnitName = readModel.UnitName;
            SewingOutId = readModel.SewingOutId;
            SewingOutNo = readModel.SewingOutNo;
            UnitFromCode = readModel.UnitFromCode;
            UnitFromName = readModel.UnitFromName;
            UnitFromId = new UnitDepartmentId(readModel.UnitFromId);
            ComodityId = new GarmentComodityId(readModel.ComodityId);
            ComodityName = readModel.ComodityName;
            ComodityCode = readModel.ComodityCode;
            FinishingInDate = readModel.FinishingInDate;
            FinishingInType = readModel.FinishingInType;
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentFinishingIn GetEntity()
        {
            return this;
        }
    }
}
