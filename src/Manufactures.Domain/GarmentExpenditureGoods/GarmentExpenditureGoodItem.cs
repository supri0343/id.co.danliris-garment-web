using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentExpenditureGoods.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentExpenditureGoods
{
    public class GarmentExpenditureGoodItem : AggregateRoot<GarmentExpenditureGoodItem, GarmentExpenditureGoodItemReadModel>
    {
        public Guid ExpenditureGoodId { get; private set; }
        public Guid FinishedGoodStockId { get; private set; }
        public SizeId SizeId { get; private set; }
        public string SizeName { get; private set; }
        public double Quantity { get; private set; }
        public UomId UomId { get; private set; }
        public string UomUnit { get; private set; }
        public string Description { get; private set; }
        public double BasicPrice { get; private set; }
        public double Price { get; private set; }

        public GarmentExpenditureGoodItem(Guid identity, Guid expenditureGoodId, Guid finishedGoodStockId, SizeId sizeId, string sizeName, double quantity, UomId uomId, string uomUnit, string description, double basicPrice, double price) : base(identity)
        {
            ExpenditureGoodId = expenditureGoodId;
            FinishedGoodStockId = finishedGoodStockId;
            SizeId = sizeId;
            SizeName = sizeName;
            Quantity = quantity;
            UomId = uomId;
            UomUnit = uomUnit;
            Description = description;
            BasicPrice = basicPrice;
            Price = price;

            ReadModel = new GarmentExpenditureGoodItemReadModel(Identity)
            {
                ExpenditureGoodId = ExpenditureGoodId,
                FinishedGoodStockId= FinishedGoodStockId,
                SizeId = SizeId.Value,
                SizeName = SizeName,
                Quantity = Quantity,
                UomId = UomId.Value,
                UomUnit = UomUnit,
                Description = Description,
                BasicPrice = BasicPrice,
                Price = Price
            };

            ReadModel.AddDomainEvent(new OnGarmentExpenditureGoodPlaced(Identity));
        }

        public GarmentExpenditureGoodItem(GarmentExpenditureGoodItemReadModel readModel) : base(readModel)
        {
            ExpenditureGoodId = readModel.ExpenditureGoodId;
            FinishedGoodStockId = readModel.FinishedGoodStockId;
            SizeId = new SizeId(readModel.SizeId);
            SizeName = readModel.SizeName;
            Quantity = readModel.Quantity;
            UomId = new UomId(readModel.UomId);
            UomUnit = readModel.UomUnit;
            Description = readModel.Description;
            BasicPrice = readModel.BasicPrice;
            Price = readModel.Price;
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentExpenditureGoodItem GetEntity()
        {
            return this;
        }
    }
}
