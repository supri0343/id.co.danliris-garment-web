using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingIns.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentSewingIns
{
    public class GarmentSubconSewingIn : AggregateRoot<GarmentSubconSewingIn, GarmentSubconSewingInReadModel>
    {
        public string SewingInNo { get; private set; }
        public string SewingFrom { get; private set; }
        public Guid LoadingOutId { get; private set; }
        public string LoadingOutNo { get; private set; }
        public UnitDepartmentId UnitFromId { get; private set; }
        public string UnitFromCode { get; private set; }
        public string UnitFromName { get; private set; }
        public UnitDepartmentId UnitId { get; private set; }
        public string UnitCode { get; private set; }
        public string UnitName { get; private set; }
        public string RONo { get; private set; }
        public string Article { get; private set; }
        public GarmentComodityId ComodityId { get; private set; }
        public string ComodityCode { get; private set; }
        public string ComodityName { get; private set; }
        public DateTimeOffset SewingInDate { get; private set; }
        public bool IsApproved { get; internal set; }

        public GarmentSubconSewingIn(Guid identity, string sewingInNo, string sewingFrom, Guid loadingOutId, string loadingOutNo, UnitDepartmentId unitFromId, string unitFromCode, string unitFromName, UnitDepartmentId unitId, string unitCode, string unitName, string roNo, string article, GarmentComodityId comodityId, string comodityCode, string comodityName, DateTimeOffset sewingInDate, bool isApproved) : base(identity)
        {
            Identity = identity;
            SewingInNo = sewingInNo;
            SewingFrom = sewingFrom;
            LoadingOutId = loadingOutId;
            LoadingOutNo = loadingOutNo;
            UnitFromId = unitFromId;
            UnitFromCode = unitFromCode;
            UnitFromName = unitFromName;
            UnitId = unitId;
            UnitCode = unitCode;
            UnitName = unitName;
            RONo = roNo;
            Article = article;
            ComodityId = comodityId;
            ComodityCode = comodityCode;
            ComodityName = comodityName;
            SewingInDate = sewingInDate;
            IsApproved = isApproved;

            ReadModel = new GarmentSubconSewingInReadModel(Identity)
            {
                SewingInNo = SewingInNo,
                SewingFrom=SewingFrom,
                LoadingOutId = LoadingOutId,
                LoadingOutNo = LoadingOutNo,
                UnitFromId = UnitFromId.Value,
                UnitFromCode = UnitFromCode,
                UnitFromName = UnitFromName,
                UnitId = UnitId.Value,
                UnitCode = UnitCode,
                UnitName = UnitName,
                RONo = RONo,
                Article = Article,
                ComodityId = ComodityId.Value,
                ComodityCode = ComodityCode,
                ComodityName = ComodityName,
                SewingInDate = SewingInDate,
                IsApproved = IsApproved
            };
            ReadModel.AddDomainEvent(new OnGarmentSewingInPlaced(Identity));
        }

        public GarmentSubconSewingIn(GarmentSubconSewingInReadModel readModel) : base(readModel)
        {
            SewingInNo = readModel.SewingInNo;
            SewingFrom = readModel.SewingFrom;
            LoadingOutId = readModel.LoadingOutId;
            LoadingOutNo = readModel.LoadingOutNo;
            UnitFromId = new UnitDepartmentId(readModel.UnitFromId);
            UnitFromCode = readModel.UnitFromCode;
            UnitFromName = readModel.UnitFromName;
            UnitId = new UnitDepartmentId(readModel.UnitId);
            UnitCode = readModel.UnitCode;
            UnitName = readModel.UnitName;
            RONo = readModel.RONo;
            Article = readModel.Article;
            ComodityId = new GarmentComodityId(readModel.ComodityId);
            ComodityCode = readModel.ComodityCode;
            ComodityName = readModel.ComodityName;
            SewingInDate = readModel.SewingInDate;
            IsApproved = readModel.IsApproved;
        }
        public void setDate(DateTimeOffset sewingInDate)
        {
            if (sewingInDate != SewingInDate)
            {
                SewingInDate = sewingInDate;
                ReadModel.SewingInDate = sewingInDate;

                MarkModified();
            }
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

        protected override GarmentSubconSewingIn GetEntity()
        {
            return this;
        }
    }
}