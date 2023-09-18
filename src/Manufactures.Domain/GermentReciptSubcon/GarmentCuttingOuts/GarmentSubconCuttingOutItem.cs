using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingOuts.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentCuttingOuts
{
    public class GarmentSubconCuttingOutItem : AggregateRoot<GarmentSubconCuttingOutItem, GarmentSubconCuttingOutItemReadModel>
    {
        public Guid CutOutId { get; private set; }
        public Guid CuttingInId { get; private set; }
        public Guid CuttingInDetailId { get; private set; }
        public ProductId ProductId { get; private set; }
        public string ProductCode { get; private set; }
        public string ProductName { get; private set; }
        public string DesignColor { get; private set; }
        public double TotalCuttingOut { get; private set; }
        public double TotalCuttingOutQuantity { get; private set; }
		public string UId { get; private set; }
        public double RealQtyOut { get; private set; }
        public GarmentSubconCuttingOutItem(Guid identity, Guid cuttingInId, Guid cuttingInDetailId, Guid cutOutId, ProductId productId, string productCode, string productName, string designColor, double totalCuttingOut,double realQtyOut) : base(identity)
        {
            //MarkTransient();

            Identity = identity;
            CuttingInId = cuttingInId;
            CuttingInDetailId = cuttingInDetailId;
            CutOutId = cutOutId;
            ProductId = productId;
            ProductCode = productCode;
            ProductName = productName;
            DesignColor = designColor;
            TotalCuttingOut = totalCuttingOut;
            RealQtyOut = realQtyOut;

            ReadModel = new GarmentSubconCuttingOutItemReadModel(identity)
            {
                CuttingInId = CuttingInId,
                CutOutId = CutOutId,
                CuttingInDetailId = CuttingInDetailId,
                ProductId = ProductId.Value,
                ProductCode = ProductCode,
                ProductName = ProductName,
                DesignColor = DesignColor,
                TotalCuttingOut = TotalCuttingOut,
                RealQtyOut = RealQtyOut,
                GarmentSubconCuttingOutDetail = new List<GarmentSubconCuttingOutDetailReadModel>()
            };

            ReadModel.AddDomainEvent(new OnGarmentSubconCuttingOutPlaced(Identity));
        }

        public GarmentSubconCuttingOutItem(GarmentSubconCuttingOutItemReadModel readModel) : base(readModel)
        {
            CuttingInId = readModel.CuttingInId;
            CutOutId = readModel.CutOutId;
            CuttingInDetailId = readModel.CuttingInDetailId;
            ProductId = new ProductId(readModel.ProductId); 
            ProductCode = readModel.ProductCode;
            ProductName = readModel.ProductName;
            DesignColor = readModel.DesignColor;
            TotalCuttingOut = readModel.TotalCuttingOut;
            RealQtyOut = readModel.RealQtyOut;
        }
        public void SetRealOutQuantity(double realQtyOut)
        {
            if (this.RealQtyOut != realQtyOut)
            {
                this.RealQtyOut = realQtyOut;
                ReadModel.RealQtyOut = realQtyOut;
            }
        }

        public void SetTotalCuttingOutQuantity(double totalCuttingOut)
        {
            if (this.TotalCuttingOut != totalCuttingOut)
            {
                this.TotalCuttingOut = totalCuttingOut;
                ReadModel.TotalCuttingOut = totalCuttingOut;
            }
        }
        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentSubconCuttingOutItem GetEntity()
        {
            return this;
        }
    }
}
