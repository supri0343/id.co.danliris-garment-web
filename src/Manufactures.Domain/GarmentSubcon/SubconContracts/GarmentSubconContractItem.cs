using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSubcon;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.GarmentSubcon.SubconContracts.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.SubconContracts
{
    public class GarmentSubconContractItem : AggregateRoot<GarmentSubconContractItem, GarmentSubconContractItemReadModel>
    {
        
        public Guid SubconContractId { get; private set; }

        public ProductId ProductId { get; private set; }
        public string ProductCode { get; private set; }
        public string ProductName { get; private set; }

        public double Quantity { get; private set; }

        public UomId UomId { get; private set; }
        public string UomUnit { get; private set; }
        public double CIFItem { get; private set; }
        public double NettWeight { get; private set; }
        public double GrossWeight { get; private set; }

        public GarmentSubconContractItem(Guid identity, Guid subconContractId, ProductId productId, string productCode, string productName, double quantity, UomId uomId, string uomUnit, double cifItem, double nettWeight, double grossWeight) : base(identity)
        {
            Identity = identity;
            SubconContractId = subconContractId;
            ProductId = productId;
            ProductCode = productCode;
            ProductName = productName;
            Quantity = quantity;
            UomId = uomId;
            UomUnit = uomUnit;
            CIFItem = cifItem;
            NettWeight = nettWeight;
            GrossWeight = grossWeight;

            ReadModel = new GarmentSubconContractItemReadModel(Identity)
            {
                SubconContractId= SubconContractId,
                Quantity = Quantity,
                UomId = uomId.Value,
                UomUnit = uomUnit,
                ProductId = ProductId.Value,
                ProductCode = ProductCode,
                ProductName = ProductName,
                CIFItem = CIFItem,
                NettWeight = NettWeight,
                GrossWeight = GrossWeight
            };

            ReadModel.AddDomainEvent(new OnGarmentSubconContractPlaced(Identity));
        }

        public GarmentSubconContractItem(GarmentSubconContractItemReadModel readModel) : base(readModel)
        {
            SubconContractId = readModel.SubconContractId;
            ProductId = new ProductId(readModel.ProductId);
            ProductCode = readModel.ProductCode;
            ProductName = readModel.ProductName;
            Quantity = readModel.Quantity;
            UomUnit = readModel.UomUnit;
            UomId = new UomId(readModel.UomId);
            CIFItem = readModel.CIFItem;
            NettWeight = readModel.NettWeight;
            GrossWeight = readModel.GrossWeight;
        }
        public void SetQuantity(double Quantity)
        {
            if (this.Quantity != Quantity)
            {
                this.Quantity = Quantity;
                ReadModel.Quantity = Quantity;
            }
        }
        public void SetCIFItem(double CIFItem)
        {
            if (this.CIFItem != CIFItem)
            {
                this.CIFItem = CIFItem;
                ReadModel.CIFItem = CIFItem;
            }
        }
        public void SetNettWeight(double NettWeight)
        {
            if (this.NettWeight != NettWeight)
            {
                this.NettWeight = NettWeight;
                ReadModel.NettWeight = NettWeight;
            }
        }

        public void SetGrossWeight(double GrossWeight)
        {
            if (this.GrossWeight != GrossWeight)
            {
                this.GrossWeight = GrossWeight;
                ReadModel.GrossWeight = GrossWeight;
            }
        }
        public void Modify()
        {
            MarkModified();
        }
        protected override GarmentSubconContractItem GetEntity()
        {
            return this;
        }
    }
}
