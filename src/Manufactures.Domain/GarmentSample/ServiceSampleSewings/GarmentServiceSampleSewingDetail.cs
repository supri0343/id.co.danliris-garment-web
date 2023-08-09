using Infrastructure.Domain;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.Events.GarmentSample;

namespace Manufactures.Domain.GarmentSample.ServiceSampleSewings
{
    public class GarmentServiceSampleSewingDetail : AggregateRoot<GarmentServiceSampleSewingDetail, GarmentServiceSampleSewingDetailReadModel>
    {

        public Guid ServiceSampleSewingItemId { get; private set; }
        public Guid SewingInId { get; private set; }
        public Guid SewingInItemId { get; private set; }
        public ProductId ProductId { get; private set; }
        public string ProductCode { get; private set; }
        public string ProductName { get; private set; }
        public string DesignColor { get; private set; }
        public double Quantity { get; private set; }
        public UomId UomId { get; private set; }
        public string UomUnit { get; private set; }
        public UnitDepartmentId UnitId { get; private set; }
        public string UnitCode { get; private set; }
        public string UnitName { get; private set; }
        public string Remark { get; private set; }
        public string Color { get; private set; }

        public GarmentServiceSampleSewingDetail(Guid identity, Guid serviceSampleSewingItemId, Guid sewingInId, Guid sewingInItemId, ProductId productId, string productCode, string productName, string designColor, double quantity, UomId uomId, string uomUnit, UnitDepartmentId unitId, string unitCode, string unitName, string remark, string color) : base(identity)
        {
            ServiceSampleSewingItemId = serviceSampleSewingItemId;
            SewingInId = sewingInId;
            SewingInItemId = sewingInItemId;
            ProductId = productId;
            ProductCode = productCode;
            ProductName = productName;
            DesignColor = designColor;
            Quantity = quantity;
            UomId = uomId;
            UomUnit = uomUnit;
            UnitId = unitId;
            UnitCode = unitCode;
            UnitName = unitName;
            Remark = remark;
            Color = color;

            ReadModel = new GarmentServiceSampleSewingDetailReadModel(identity)
            {
                ServiceSampleSewingItemId = ServiceSampleSewingItemId,
                SewingInId = SewingInId,
                SewingInItemId = SewingInItemId,
                ProductId = ProductId.Value,
                ProductCode = ProductCode,
                ProductName = ProductName,
                DesignColor = DesignColor,
                Quantity = Quantity,
                UomId = UomId.Value,
                UomUnit = UomUnit,
                UnitId = UnitId.Value,
                UnitCode = UnitCode,
                UnitName = UnitName,
                Remark = Remark,
                Color = Color
            };

            ReadModel.AddDomainEvent(new OnGarmentServiceSampleSewingPlaced(Identity));
        }

        public GarmentServiceSampleSewingDetail(GarmentServiceSampleSewingDetailReadModel readModel) : base(readModel)
        {
            ServiceSampleSewingItemId = readModel.ServiceSampleSewingItemId;
            SewingInId = readModel.SewingInId;
            SewingInItemId = readModel.SewingInItemId;
            ProductId = new ProductId(readModel.ProductId);
            ProductCode = readModel.ProductCode;
            ProductName = readModel.ProductName;
            DesignColor = readModel.DesignColor;
            Quantity = readModel.Quantity;
            UomId = new UomId(readModel.UomId);
            UomUnit = readModel.UomUnit;
            UnitCode = readModel.UnitCode;
            UnitId = new UnitDepartmentId(readModel.UnitId);
            UnitName = readModel.UnitName;
            Remark = readModel.Remark;
            Color = readModel.Color;
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentServiceSampleSewingDetail GetEntity()
        {
            return this;
        }

        public void SetQuantity(double Quantity)
        {
            if (this.Quantity != Quantity)
            {
                this.Quantity = Quantity;
                ReadModel.Quantity = Quantity;
            }
        }

        public void SetUomId(UomId uomId)
        {
            if (UomId != uomId)
            {
                UomId = uomId;
                ReadModel.UomId = uomId.Value;
            }
        }

        public void SetUomUnit(string uomUnit)
        {
            if (UomUnit != uomUnit)
            {
                UomUnit = uomUnit;
                ReadModel.UomUnit = uomUnit;
            }
        }

        public void SetRemark(string remark)
        {
            if (this.Remark != remark)
            {
                this.Remark = remark;
                ReadModel.Remark = remark;
            }
        }
    }
}
