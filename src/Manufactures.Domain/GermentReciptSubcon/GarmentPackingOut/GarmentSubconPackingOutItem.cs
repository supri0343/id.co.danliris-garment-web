using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.Events.GarmentReceiptSubcon;
using Manufactures.Domain.GarmentPackingOut.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentPackingOut
{
    public class GarmentSubconPackingOutItem : AggregateRoot<GarmentSubconPackingOutItem, GarmentSubconPackingOutItemReadModel>
    {
        public Guid PackingOutId { get; private set; }
        public Guid PackingInItemId { get; private set; }
        public SizeId SizeId { get; private set; }
        public string SizeName { get; private set; }
        public double Quantity { get; private set; }
        public double ReturQuantity { get; private set; }
        public UomId UomId { get; private set; }
        public string UomUnit { get; private set; }
        public string Description { get; private set; }
        public double BasicPrice { get; private set; }
        public double Price { get; private set; }
        public Guid FinishedGoodStockId { get; private set; }
        //public bool IsPackingList{ get; private set; }
        public GarmentSubconPackingOutItem(Guid identity, Guid packingOutId, Guid packingInItemId, SizeId sizeId, string sizeName, double quantity, double returQuantity, UomId uomId, string uomUnit, string description, double basicPrice, double price, Guid finishedGoodStockId) : base(identity)
        {
            PackingOutId = packingOutId;
            PackingInItemId = packingInItemId;
            SizeId = sizeId;
            SizeName = sizeName;
            Quantity = quantity;
            ReturQuantity = returQuantity;
            UomId = uomId;
            UomUnit = uomUnit;
            Description = description;
            BasicPrice = basicPrice;
            Price = price;
            FinishedGoodStockId = finishedGoodStockId;
            //IsPackingList = isPackingList;

            ReadModel = new GarmentSubconPackingOutItemReadModel(Identity)
            {
                PackingOutId = PackingOutId,
                PackingInItemId = PackingInItemId,
                SizeId = SizeId.Value,
                SizeName = SizeName,
                Quantity = Quantity,
                ReturQuantity = ReturQuantity,
                UomId = UomId.Value,
                UomUnit = UomUnit,
                Description = Description,
                BasicPrice = BasicPrice,
                Price = Price,
                FinishedGoodStockId = FinishedGoodStockId,
                //IsPackingList = IsPackingList
        };

            ReadModel.AddDomainEvent(new OnGarmentSubconPackingOutPlaced(Identity));
        }

        public GarmentSubconPackingOutItem(GarmentSubconPackingOutItemReadModel readModel) : base(readModel)
        {
            PackingOutId = readModel.PackingOutId;
            PackingInItemId = readModel.PackingInItemId;
            SizeId = new SizeId(readModel.SizeId);
            SizeName = readModel.SizeName;
            Quantity = readModel.Quantity;
            ReturQuantity = readModel.ReturQuantity;
            UomId = new UomId(readModel.UomId);
            UomUnit = readModel.UomUnit;
            Description = readModel.Description;
            BasicPrice = readModel.BasicPrice;
            Price = readModel.Price;
            FinishedGoodStockId = readModel.FinishedGoodStockId;
            //IsPackingList = readModel.IsPackingList;
        }

        public void SetReturQuantity(double ReturQuantity)
        {
            if (this.ReturQuantity != ReturQuantity)
            {
                this.ReturQuantity = ReturQuantity;
                ReadModel.ReturQuantity = ReturQuantity;
            }
        }

        //public void SetIsPackingList(bool isPackingList)
        //{
        //    if (this.IsPackingList != isPackingList)
        //    {
        //        this.IsPackingList = isPackingList;
        //        ReadModel.IsPackingList = isPackingList;
        //    }
        //}

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentSubconPackingOutItem GetEntity()
        {
            return this;
        }
    }
}
