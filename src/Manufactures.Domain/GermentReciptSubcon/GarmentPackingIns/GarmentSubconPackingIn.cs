using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentReceiptSubcon;
using Manufactures.Domain.GermentReciptSubcon.GarmentPackingIns.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentPackingIns
{
    public class GarmentSubconPackingIn : AggregateRoot<GarmentSubconPackingIn, GarmentSubconPackingInReadModel>
    {
        public string PackingInNo { get; private set; }
        public UnitDepartmentId UnitId { get; private set; }
        public string UnitCode { get; private set; }
        public string UnitName { get; private set; }
        public UnitDepartmentId UnitFromId { get; private set; }
        public string UnitFromCode { get; private set; }
        public string UnitFromName { get; private set; }
        public string PackingFrom { get; private set; }
        public string RONo { get; private set; }
        public string Article { get; private set; }
        public GarmentComodityId ComodityId { get; private set; }
        public string ComodityCode { get; private set; }
        public string ComodityName { get; private set; }
        public DateTimeOffset PackingInDate { get; private set; }
        public bool IsApproved { get; private set; }

        public GarmentSubconPackingIn(Guid identity, string packingInNo, UnitDepartmentId unitId, string unitCode, string unitName, UnitDepartmentId unitFromId, string unitFromCode, string unitFromName, string packingFrom, string rONo, string article, GarmentComodityId comodityId, string comodityCode, string comodityName, DateTimeOffset packingInDate, bool isApproved) : base(identity)
        {
            Validator.ThrowIfNull(() => unitId);
            Validator.ThrowIfNull(() => unitFromId);
            Validator.ThrowIfNull(() => rONo);

            Identity = identity;
            PackingInNo = packingInNo;
            UnitId = unitId;
            UnitCode = unitCode;
            UnitName = unitName;
            UnitFromId = unitFromId;
            UnitFromCode = unitFromCode;
            UnitFromName = unitFromName;
            PackingFrom = packingFrom;
            RONo = rONo;
            Article = article;
            ComodityId = comodityId;
            ComodityCode = comodityCode;
            ComodityName = comodityName;
            PackingInDate = packingInDate;
            IsApproved = isApproved;

            ReadModel = new GarmentSubconPackingInReadModel(Identity)
            {
                PackingInNo = PackingInNo,
                UnitId = UnitId.Value,
                UnitCode = UnitCode,
                UnitName = UnitName,
                UnitFromId = UnitFromId.Value,
                UnitFromCode = UnitFromCode,
                UnitFromName = UnitFromName,
                PackingFrom = PackingFrom,
                RONo = RONo,
                Article = Article,
                ComodityId = ComodityId.Value,
                ComodityCode = ComodityCode,
                ComodityName = ComodityName,
                PackingInDate = PackingInDate,
                IsApproved = IsApproved,

            };
            ReadModel.AddDomainEvent(new OnGarmentSubconPackingInPlaced(Identity));
        }

        public GarmentSubconPackingIn(GarmentSubconPackingInReadModel readModel) : base(readModel)
        {
            PackingInNo = readModel.PackingInNo;
            UnitId = new UnitDepartmentId(readModel.UnitId);
            UnitCode = readModel.UnitCode;
            UnitName = readModel.UnitName;
            UnitFromId = new UnitDepartmentId(readModel.UnitFromId);
            UnitFromCode = readModel.UnitFromCode;
            UnitFromName = readModel.UnitFromName;
            PackingFrom = readModel.PackingFrom;
            RONo = readModel.RONo;
            Article = readModel.Article;
            ComodityId = new GarmentComodityId(readModel.ComodityId);
            ComodityCode = readModel.ComodityCode;
            ComodityName = readModel.ComodityName;
            PackingInDate = readModel.PackingInDate;
            IsApproved = readModel.IsApproved;
        }

        public void setApproved(bool isApproved)
        {
            if (isApproved != IsApproved)
            {
                IsApproved = isApproved;
                ReadModel.IsApproved = isApproved;

                MarkModified();
            }
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentSubconPackingIn GetEntity()
        {
            return this;
        }

    }
}
