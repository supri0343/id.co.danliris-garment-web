using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentLoadings.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingOuts.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentLoadingOuts
{
    public class GarmentSubconLoadingOut : AggregateRoot<GarmentSubconLoadingOut, GarmentSubconLoadingOutReadModel>
    {
        public string LoadingOutNo { get; internal set; }
        public Guid LoadingInId { get; internal set; }
        public string LoadingInNo { get; internal set; }
        public UnitDepartmentId UnitFromId { get; internal set; }
        public string UnitFromCode { get; internal set; }
        public string UnitFromName { get; internal set; }
        public UnitDepartmentId UnitId { get; internal set; }
        public string UnitCode { get; internal set; }
        public string UnitName { get; internal set; }
        public string RONo { get; internal set; }
        public string Article { get; internal set; }
        public GarmentComodityId ComodityId { get; internal set; }
        public string ComodityCode { get; internal set; }
        public string ComodityName { get; internal set; }
        public DateTimeOffset LoadingOutDate { get; internal set; }

        public GarmentSubconLoadingOut(Guid identity, string loadingNo, Guid cuttingOutId, string cuttingOutNo, UnitDepartmentId unitFromId, string unitFromCode, string unitFromName, string rONo, string article, UnitDepartmentId unitId, string unitCode, string unitName, DateTimeOffset loadingDate, GarmentComodityId comodityId, string comodityCode, string comodityName) : base(identity)
        {
            Validator.ThrowIfNull(() => unitId);
            //Validator.ThrowIfNull(() => sewingDOId);

            //MarkTransient();
            LoadingOutNo = loadingNo;
            Identity = identity;
            LoadingInId = cuttingOutId;
            LoadingInNo = cuttingOutNo;
            UnitFromCode = unitFromCode;
            UnitFromName = unitFromName;
            UnitFromId = unitFromId;
            RONo = rONo;
            Article = article;
            UnitId = unitId;
            UnitCode = unitCode;
            UnitName = unitName;
            ComodityId = comodityId;
            LoadingOutDate = loadingDate;
            ComodityCode = comodityCode;
            ComodityName = comodityName;


            ReadModel = new GarmentSubconLoadingOutReadModel(Identity)
            {
                LoadingOutDate = LoadingOutDate,
                LoadingOutNo = LoadingOutNo,
                RONo = RONo,
                Article = Article,
                UnitId = UnitId.Value,
                UnitCode = UnitCode,
                UnitName = UnitName,
                LoadingInId = LoadingInId,
                LoadingInNo = LoadingInNo,
                UnitFromCode = UnitFromCode,
                UnitFromName = UnitFromName,
                UnitFromId = UnitFromId.Value,
                ComodityId=ComodityId.Value,
                ComodityCode=ComodityCode,
                ComodityName=ComodityName
            };

            ReadModel.AddDomainEvent(new OnGarmentLoadingPlaced(Identity));
        }

        public GarmentSubconLoadingOut(GarmentSubconLoadingOutReadModel readModel) : base(readModel)
        {
            LoadingOutNo = readModel.LoadingOutNo;
            RONo = readModel.RONo;
            Article = readModel.Article;
            UnitId = new UnitDepartmentId(readModel.UnitId);
            UnitCode = readModel.UnitCode;
            UnitName = readModel.UnitName;
            LoadingInId = readModel.LoadingInId;
            LoadingInNo = readModel.LoadingInNo;
            UnitFromCode = readModel.UnitFromCode;
            UnitFromName = readModel.UnitFromName;
            UnitFromId = new UnitDepartmentId(readModel.UnitFromId);
            ComodityId = new GarmentComodityId(readModel.ComodityId);
            ComodityName = readModel.ComodityName;
            ComodityCode = readModel.ComodityCode;
            LoadingOutDate = readModel.LoadingOutDate;
        }
        public void setDate(DateTimeOffset loadingDate)
        {
            if (loadingDate != LoadingOutDate)
            {
                LoadingOutDate = loadingDate;
                ReadModel.LoadingOutDate = loadingDate;

                MarkModified();
            }
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentSubconLoadingOut GetEntity()
        {
            return this;
        }
    }
}
