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
        public string FinishingInNo { get; private set; }
        public string FinishingInType { get; private set; }
        public UnitDepartmentId UnitId { get; private set; }
        public string UnitCode { get; private set; }
        public string UnitName { get; private set; }
        public UnitDepartmentId UnitFromId { get; private set; }
        public string UnitFromCode { get; private set; }
        public string UnitFromName { get; private set; }
        public string Article { get; private set; }
        public string RONo { get; private set; }
        public GarmentComodityId ComodityId { get; private set; }
        public string ComodityCode { get; private set; }
        public string ComodityName { get; private set; }
        public DateTimeOffset FinishingInDate { get; private set; }

        public GarmentFinishingIn(Guid identity, string finishingInNo, string finishingInType, UnitDepartmentId unitFromId, string unitFromCode, string unitFromName, string rONo, string article, UnitDepartmentId unitId, string unitCode, string unitName, DateTimeOffset finishingInDate, GarmentComodityId comodityId, string comodityCode, string comodityName) : base(identity)
        {
            Validator.ThrowIfNull(() => unitId);

            //MarkTransient();
            FinishingInNo = finishingInNo;
            Identity = identity;
            FinishingInType = finishingInType;
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
