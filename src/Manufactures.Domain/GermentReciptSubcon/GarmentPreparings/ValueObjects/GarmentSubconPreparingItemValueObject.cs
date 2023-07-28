using Manufactures.Domain.GarmentPreparings.ValueObjects;
using Moonlay.Domain;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentPreparings.ValueObjects
{
    public class GarmentSubconPreparingItemValueObject : ValueObject
    {
        public GarmentSubconPreparingItemValueObject()
        {

        }


        public GarmentSubconPreparingItemValueObject(Guid id, int uenItemId, Product product, string designColor, double quantity, Uom uom, string fabricType, double remainingQuantity, double basicPrice, Guid garmentPreparingId, string roSource,string beacukaiNo, DateTimeOffset beacukaiDate, string beacukaiType)
        {
            Identity = id;
            UENItemId = uenItemId;
            Product = product;
            DesignColor = designColor;
            Quantity = quantity;
            Uom = uom;
            FabricType = fabricType;
            RemainingQuantity = remainingQuantity;
            BasicPrice = basicPrice;
            GarmentPreparingId = garmentPreparingId;
            ROSource = roSource;
            BeacukaiNo = beacukaiNo;
            BeacukaiDate = beacukaiDate;
            BeacukaiType = beacukaiType;
        }

        public Guid Identity { get; set; }
        public int UENItemId { get; set; }
        public Product Product { get; set; }
        public string DesignColor { get; set; }
        public double Quantity { get; set; }
        public Uom Uom { get; set; }
        public string FabricType { get; set; }
        public double RemainingQuantity { get; set; }
        public double BasicPrice { get; set; }
        public Guid GarmentPreparingId { get; set; }
        public string ROSource { get; set; }
        public string BeacukaiNo { get;  set; }
        public DateTimeOffset BeacukaiDate { get;  set; }
        public string BeacukaiType { get;  set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}