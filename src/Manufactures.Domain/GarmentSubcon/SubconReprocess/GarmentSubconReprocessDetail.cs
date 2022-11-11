using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSubcon.SubconReprocess;
using Manufactures.Domain.GarmentSubcon.SubconReprocess.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.SubconReprocess
{
    public class GarmentSubconReprocessDetail : AggregateRoot<GarmentSubconReprocessDetail, GarmentSubconReprocessDetailReadModel>
    {

        public Guid ReprocessItemId { get; internal set; }
        public SizeId SizeId { get; internal set; }
        public string SizeName { get; internal set; }
        public double Quantity { get; internal set; }
        public double ReprocessQuantity { get; internal set; }
        public UomId UomId { get; internal set; }
        public string UomUnit { get; internal set; }
        public string Color { get; internal set; }
        public string DesignColor { get; internal set; }

        //KOMPONEN (CUTTING)
        public Guid ServiceSubconCuttingDetailId { get; internal set; }
        public Guid ServiceSubconCuttingSizeId { get; internal set; }

        //WASH (SEWING)
        public Guid ServiceSubconSewingDetailId { get; internal set; }

        public UnitDepartmentId UnitId { get; private set; }
        public string UnitCode { get; private set; }
        public string UnitName { get; private set; }
        public string Remark { get; private set; }

        public GarmentSubconReprocessDetail(Guid identity, Guid reprocessItemId, SizeId sizeId, string sizeName, double quantity, double reprocessQuantity, UomId uomId, string uomUnit, string color, Guid serviceSubconCuttingDetailId, Guid serviceSubconCuttingSizeId, Guid serviceSubconSewingDetailId, string designColor, UnitDepartmentId unitId, string unitCode, string unitName, string remark) : base(identity)
        {
            Identity = identity;
            ReprocessItemId = reprocessItemId;
            SizeId = sizeId;
            SizeName = sizeName;
            Quantity = quantity;
            ReprocessQuantity = reprocessQuantity;
            UomId = uomId;
            UomUnit = uomUnit;
            Color = color;
            ServiceSubconCuttingDetailId = serviceSubconCuttingDetailId;
            ServiceSubconCuttingSizeId = serviceSubconCuttingSizeId;
            DesignColor = designColor;
            ServiceSubconSewingDetailId = serviceSubconSewingDetailId;
            UnitId = unitId;
            UnitCode = unitCode;
            UnitName = unitName;
            Remark = remark;

            ReadModel = new GarmentSubconReprocessDetailReadModel(Identity)
            {
                ReprocessItemId = ReprocessItemId,
                SizeId = SizeId != null ?SizeId.Value :0,
                SizeName = SizeName,
                Quantity= Quantity,
                ReprocessQuantity= ReprocessQuantity,
                UomId= UomId.Value,
                UomUnit= UomUnit,
                Color= Color,
                ServiceSubconCuttingDetailId= ServiceSubconCuttingDetailId,
                ServiceSubconCuttingSizeId= ServiceSubconCuttingSizeId,
                DesignColor= DesignColor,
                ServiceSubconSewingDetailId= ServiceSubconSewingDetailId,
                UnitId = UnitId != null ?UnitId.Value :0,
                UnitCode = UnitCode,
                UnitName = UnitName,
                Remark = Remark,
            };

            ReadModel.AddDomainEvent(new OnGarmentSubconReprocessPlaced(Identity));
        }

        public GarmentSubconReprocessDetail(GarmentSubconReprocessDetailReadModel readModel) : base(readModel)
        {
            ReprocessItemId = readModel.ReprocessItemId;
            SizeId = new SizeId(readModel.SizeId);
            SizeName = readModel.SizeName;
            Quantity = readModel.Quantity;
            ReprocessQuantity = readModel.ReprocessQuantity;
            UomId = new UomId(readModel.UomId);
            UomUnit = readModel.UomUnit;
            Color = readModel.Color;
            ServiceSubconCuttingDetailId = readModel.ServiceSubconCuttingDetailId;
            ServiceSubconCuttingSizeId = readModel.ServiceSubconCuttingSizeId;
            ServiceSubconSewingDetailId = readModel.ServiceSubconSewingDetailId;
            DesignColor = readModel.DesignColor;
            UnitCode = readModel.UnitCode;
            UnitId = new UnitDepartmentId(readModel.UnitId);
            UnitName = readModel.UnitName;
            Remark = readModel.Remark;
        }
        public void Modify()
        {
            MarkModified();
        }

        public void SetQuantity(double Quantity)
        {
            if (this.Quantity != Quantity)
            {
                this.Quantity = Quantity;
                ReadModel.Quantity = Quantity;
            }
        }

        public void SetRemark(string Remark)
        {
            if (this.Remark != Remark)
            {
                this.Remark = Remark;
                ReadModel.Remark = Remark;
            }
        }

        public void SetColor(string Color)
        {
            if (this.Color != Color)
            {
                this.Color = Color;
                ReadModel.Color = Color;
            }
        }


        protected override GarmentSubconReprocessDetail GetEntity()
        {
            return this;
        }
    }
}
