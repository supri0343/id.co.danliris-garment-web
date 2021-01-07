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
        public string RONo { get; private set; }
        public string Article { get; private set; }
        public UnitDepartmentId UnitId { get; private set; }
        public string UnitCode { get; private set; }
        public string UnitName { get; private set; }
        public GarmentComodityId ComodityId { get; private set; }
        public string ComodityCode { get; private set; }
        public string ComodityName { get; private set; }
        public DateTimeOffset SubconDate { get; private set; }

        public GarmentServiceSubconCutting(Guid identity, string subconNo, string subconType, string rONo, string article, UnitDepartmentId unitId, string unitCode, string unitName, GarmentComodityId comodityId, string comodityCode, string comodityName, DateTimeOffset subconDate) : base(identity)
        {
            Identity = identity;
            SubconNo = subconNo;
            SubconType = subconType;
            RONo = rONo;
            Article = article;
            UnitId = unitId;
            UnitCode = unitCode;
            UnitName = unitName;
            ComodityId = comodityId;
            ComodityCode = comodityCode;
            ComodityName = comodityName;
            SubconDate = subconDate;

            ReadModel = new GarmentServiceSubconCuttingReadModel(Identity)
            {
                SubconDate = SubconDate,
                Article = Article,
                ComodityCode = ComodityCode,
                ComodityId = ComodityId.Value,
                ComodityName = ComodityName,
                RONo = RONo,
                SubconNo = SubconNo,
                SubconType = SubconType,
                UnitCode = UnitCode,
                UnitId = UnitId.Value,
                UnitName = UnitName

            };

            ReadModel.AddDomainEvent(new OnServiceSubconCuttingPlaced(Identity));
        }

        public GarmentServiceSubconCutting(GarmentServiceSubconCuttingReadModel readModel) : base(readModel)
        {
            UnitName = readModel.UnitName;
            ComodityCode = readModel.ComodityCode;
            UnitId = new UnitDepartmentId(readModel.UnitId);
            ComodityName = readModel.ComodityName;
            UnitCode = readModel.UnitCode;
            ComodityId = new GarmentComodityId(readModel.ComodityId);
            Article = readModel.Article;
            SubconDate = readModel.SubconDate;
            SubconNo = readModel.SubconNo;
            SubconType = readModel.SubconType;
            RONo = readModel.RONo;

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
