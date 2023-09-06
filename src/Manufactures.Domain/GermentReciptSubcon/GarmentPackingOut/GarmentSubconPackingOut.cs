using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.Events.GarmentReceiptSubcon;
using Manufactures.Domain.GarmentPackingOut.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentPackingOut
{
    public class GarmentSubconPackingOut : AggregateRoot<GarmentSubconPackingOut, GarmentSubconPackingOutReadModel>
    {

        public string PackingOutNo { get; private set; }
        public UnitDepartmentId UnitId { get; private set; }
        public string UnitCode { get; private set; }
        public string UnitName { get; private set; }
        public string PackingOutType { get; private set; }
        public string RONo { get; private set; }
        public string Article { get; private set; }
        public GarmentComodityId ComodityId { get; private set; }
        public string ComodityCode { get; private set; }
        public string ComodityName { get; private set; }
        public Shared.ValueObjects.BuyerId ProductOwnerId { get; private set; }
        public string ProductOwnerCode { get; private set; }
        public string ProductOwnerName { get; private set; }
        public DateTimeOffset PackingOutDate { get; private set; }
        public string Invoice { get; private set; }
        public int PackingListId { get; private set; }
        public string ContractNo { get; private set; }
        public double Carton { get; private set; }
        public string Description { get; private set; }
        public bool IsReceived { get; private set; }

        public GarmentSubconPackingOut(Guid identity, string packingOutNoNo, string packingOutType, UnitDepartmentId unitId, string unitCode, string unitName, string rONo, string article, GarmentComodityId comodityId, string comodityCode, string comodityName, BuyerId productOwnerId, string productOwnerCode, string productOwnerName, DateTimeOffset packingOutDate, string invoice, string contractNo, double carton, string description, bool isReceived, int packingListId) : base(identity)
        {
            Validator.ThrowIfNull(() => unitId);

            //MarkTransient();
            PackingOutNo = packingOutNoNo;
            Identity = identity;
            PackingOutType = packingOutType;
            UnitId = unitId;
            UnitCode = unitCode;
            UnitName = unitName;
            RONo = rONo;
            Article = article;
            ComodityId = comodityId;
            ComodityCode = comodityCode;
            ComodityName = comodityName;
            ProductOwnerId = productOwnerId;
            ProductOwnerCode = productOwnerCode;
            ProductOwnerName = productOwnerName;
            PackingOutDate = packingOutDate;
            Invoice = invoice;
            ContractNo = contractNo;
            Carton = carton;
            Description = description;
            IsReceived = isReceived;
            PackingListId = packingListId;

            ReadModel = new GarmentSubconPackingOutReadModel(Identity)
            {
                PackingOutNo = PackingOutNo,
                PackingOutType = PackingOutType,
                PackingOutDate = PackingOutDate,
                RONo = RONo,
                Article = Article,
                UnitId = UnitId.Value,
                UnitCode = UnitCode,
                UnitName = UnitName,
                ProductOwnerCode = ProductOwnerCode,
                ProductOwnerName = ProductOwnerName,
                ProductOwnerId = ProductOwnerId.Value,
                ComodityId = ComodityId.Value,
                ComodityCode = ComodityCode,
                ComodityName = ComodityName,
                Invoice = Invoice,
                ContractNo = ContractNo,
                Carton= Carton,
                Description= Description,
                IsReceived= IsReceived,
                PackingListId = PackingListId
            };

            ReadModel.AddDomainEvent(new OnGarmentSubconPackingOutPlaced(Identity));
        }

        public GarmentSubconPackingOut(GarmentSubconPackingOutReadModel readModel) : base(readModel)
        {
            PackingOutNo = readModel.PackingOutNo;
            RONo = readModel.RONo;
            Article = readModel.Article;
            UnitId = new UnitDepartmentId(readModel.UnitId);
            UnitCode = readModel.UnitCode;
            UnitName = readModel.UnitName;
            ProductOwnerCode = readModel.ProductOwnerCode;
            ProductOwnerName = readModel.ProductOwnerName;
            ProductOwnerId = new BuyerId(readModel.ProductOwnerId);
            ComodityId = new GarmentComodityId(readModel.ComodityId);
            ComodityName = readModel.ComodityName;
            ComodityCode = readModel.ComodityCode;
            PackingOutDate = readModel.PackingOutDate;
            PackingOutType = readModel.PackingOutType;
            Invoice = readModel.Invoice;
            ContractNo = readModel.ContractNo;
            Carton = readModel.Carton;
            Description = readModel.Description;
            IsReceived = readModel.IsReceived;
            PackingListId = readModel.PackingListId;
        }

        public void SetCarton(double Carton)
        {
            if (this.Carton != Carton)
            {
                this.Carton = Carton;
                ReadModel.Carton = Carton;
            }
        }

        public void SetInvoice(string Invoice)
        {
            if (this.Invoice != Invoice)
            {
                this.Invoice = Invoice;
                ReadModel.Invoice = Invoice;
            }
        }
        public void SetPackingListId(int packingListId)
        {
            if (this.PackingListId != packingListId)
            {
                this.PackingListId = packingListId;
                ReadModel.PackingListId = packingListId;
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

        public void SetExpenditureDate(DateTimeOffset PackingOutDate)
        {
            if (this.PackingOutDate != PackingOutDate)
            {
                this.PackingOutDate = PackingOutDate;
                ReadModel.PackingOutDate = PackingOutDate;
            }
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentSubconPackingOut GetEntity()
        {
            return this;
        }
    }
}
