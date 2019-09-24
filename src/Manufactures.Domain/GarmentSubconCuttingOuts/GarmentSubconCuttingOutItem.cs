using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubconCuttingOuts
{
    public class GarmentSubconCuttingOutItem : AggregateRoot<GarmentSubconCuttingOutItem, GarmentCuttingOutItemReadModel>
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

        public long EPOId { get; private set; }
        public long EPOItemId { get; private set; }
        public string POSerialNumber { get; private set; }

        public GarmentSubconCuttingOutItem(Guid identity, Guid cuttingInId, Guid cuttingInDetailId, Guid cutOutId, ProductId productId, string productCode, string productName, string designColor, double totalCuttingOut, long epoId, long epoItemId, string poSerialNumber) : base(identity)
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
            EPOId = epoId;
            EPOItemId = epoItemId;
            POSerialNumber = poSerialNumber;

            ReadModel = new GarmentCuttingOutItemReadModel(identity)
            {
                CuttingInId = CuttingInId,
                CutOutId = CutOutId,
                CuttingInDetailId = CuttingInDetailId,
                ProductId = ProductId.Value,
                ProductCode = ProductCode,
                ProductName = ProductName,
                DesignColor = DesignColor,
                TotalCuttingOut = TotalCuttingOut,
                EPOId = EPOId,
                EPOItemId = EPOItemId,
                POSerialNumber = POSerialNumber
            };

            ReadModel.AddDomainEvent(new OnGarmentSubconCuttingOutPlaced(Identity));
        }

        public GarmentSubconCuttingOutItem(GarmentCuttingOutItemReadModel readModel) : base(readModel)
        {
            CuttingInId = readModel.CuttingInId;
            CutOutId = readModel.CutOutId;
            CuttingInDetailId = readModel.CuttingInDetailId;
            ProductId = new ProductId(readModel.ProductId);
            ProductCode = readModel.ProductCode;
            ProductName = readModel.ProductName;
            DesignColor = readModel.DesignColor;
            TotalCuttingOut = readModel.TotalCuttingOut;
            EPOId = readModel.EPOId;
            EPOItemId = readModel.EPOItemId;
            POSerialNumber = readModel.POSerialNumber;
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
