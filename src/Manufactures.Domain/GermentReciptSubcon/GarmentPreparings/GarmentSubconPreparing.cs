using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentPreparings.ReadModels;
using Manufactures.Domain.GarmentPreparings.ValueObjects;
using Manufactures.Domain.GermentReciptSubcon.GarmentPreparings.ReadModels;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentPreparings
{
    public class GarmentSubconPreparing : AggregateRoot<GarmentSubconPreparing, GarmentSubconPreparingReadModel>
    {
        public int UENId { get; private set; }
        public string UENNo { get; private set; }
        public UnitDepartmentId UnitId { get; private set; }
        public string UnitCode { get; internal set; }
        public string UnitName { get; internal set; }
        public DateTimeOffset? ProcessDate { get; private set; }
        public string RONo { get; private set; }
        public string Article { get; private set; }
        public bool IsCuttingIn { get; private set; }
        public Shared.ValueObjects.BuyerId ProductOwnerId { get; private set; }
        public string ProductOwnerCode { get; private set; }
        public string ProductOwnerName { get; private set; }
		public string UId { get; private set; }
		public GarmentSubconPreparing(Guid identity, int uenId, string uenNo, UnitDepartmentId unitId, string unitCode, string unitName, DateTimeOffset? processDate, string roNo, string article, bool isCuttingIn, Shared.ValueObjects.BuyerId buyerId, string buyerCode, string buyerName) : base(identity)
        {
            this.MarkTransient();

            Identity = identity;
            UENId = uenId;
            UENNo = uenNo;
            UnitId = unitId;
            UnitCode = unitCode;
            UnitName = unitName;
            ProcessDate = processDate;
            RONo = roNo;
            Article = article;
            IsCuttingIn = isCuttingIn;
            ProductOwnerId = buyerId;
            ProductOwnerCode = buyerCode;
            ProductOwnerName = buyerName;

            ReadModel = new GarmentSubconPreparingReadModel(Identity)
            {
                UENId = UENId,
                UENNo = UENNo,
                UnitId = UnitId.Value,
                UnitCode = UnitCode,
                UnitName = UnitName,
                ProcessDate = ProcessDate,
                RONo = RONo,
                Article = Article,
                IsCuttingIn = IsCuttingIn,
                ProductOwnerId=ProductOwnerId.Value,
                ProductOwnerName=ProductOwnerName,
                ProductOwnerCode=ProductOwnerCode
            };
            ReadModel.AddDomainEvent(new OnGarmentPreparingPlaced(this.Identity));
        }

        public GarmentSubconPreparing(GarmentSubconPreparingReadModel readModel) : base(readModel)
        {
            UENId = ReadModel.UENId;
            UENNo = ReadModel.UENNo;
            UnitId = new UnitDepartmentId(ReadModel.UnitId);
            UnitCode = ReadModel.UnitCode;
            UnitName = ReadModel.UnitName;
            ProcessDate = ReadModel.ProcessDate;
            RONo = ReadModel.RONo;
            Article = ReadModel.Article;
            IsCuttingIn = ReadModel.IsCuttingIn;
            ProductOwnerCode = ReadModel.ProductOwnerCode;
            ProductOwnerId = new Shared.ValueObjects.BuyerId(ReadModel.ProductOwnerId);
            ProductOwnerName = ReadModel.ProductOwnerName;
        }

        public void setUENId(int newUENId)
        {
            //Validator.ThrowIfNull(() => newUENId);

            if (newUENId != UENId)
            {
                UENId = newUENId;
                ReadModel.UENId = newUENId;
            }
        }

        public void setUENNo(string newUENNo)
        {
            Validator.ThrowIfNullOrEmpty(() => newUENNo);

            if(newUENNo != UENNo)
            {
                UENNo = newUENNo;
                ReadModel.UENNo = newUENNo;
            }
        }

        public void SetUnitId (UnitDepartmentId newUnitId)
        {
            Validator.ThrowIfNull(() => newUnitId);

            if(newUnitId != UnitId)
            {
                UnitId = newUnitId;
                ReadModel.UnitId = newUnitId.Value;
            }
        }

        public void setUnitCode(string newUnitCode)
        {
            Validator.ThrowIfNullOrEmpty(() => newUnitCode);

            if (newUnitCode != UnitCode)
            {
                UnitCode = newUnitCode;
                ReadModel.UnitCode = newUnitCode;
            }
        }

        public void setUnitName(string newUnitName)
        {
            Validator.ThrowIfNullOrEmpty(() => newUnitName);

            if (newUnitName != UnitName)
            {
                UnitName = newUnitName;
                ReadModel.UnitName = newUnitName;
            }
        }

        public void setProcessDate(DateTimeOffset? newProcessDate)
        {
            if (newProcessDate != ProcessDate)
            {
                ProcessDate = newProcessDate;
                ReadModel.ProcessDate = newProcessDate;

                MarkModified();
            }
        }

        public void setRONo(string newRONo)
        {
            Validator.ThrowIfNullOrEmpty(() => newRONo);

            if (newRONo != RONo)
            {
                RONo = newRONo;
                ReadModel.RONo = newRONo;
            }
        }

        public void setArticle(string newArticle)
        {
            Validator.ThrowIfNullOrEmpty(() => newArticle);

            if (newArticle != Article)
            {
                Article = newArticle;
                ReadModel.Article = newArticle;
            }
        }

        public void setIsCuttingIN(bool newIsCuttingIn)
        {
            if (newIsCuttingIn != IsCuttingIn)
            {
                IsCuttingIn = newIsCuttingIn;
                ReadModel.IsCuttingIn = newIsCuttingIn;
            }
        }

        public void SetModified()
        {
            MarkModified();
        }

        protected override GarmentSubconPreparing GetEntity()
        {
            return this;
        }
    }
}