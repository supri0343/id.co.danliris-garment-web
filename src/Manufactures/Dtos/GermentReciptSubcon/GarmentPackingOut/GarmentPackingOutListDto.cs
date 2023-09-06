using Manufactures.Domain.GarmentPackingOut;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GermentReciptSubcon.GarmentPackingOut
{
    public class GarmentPackingOutListDto : BaseDto
    {
        public GarmentPackingOutListDto(GarmentSubconPackingOut garmentSubconPackingOut)
        {
            Id = garmentSubconPackingOut.Identity;
            PackingOutNo = garmentSubconPackingOut.PackingOutNo;
            RONo = garmentSubconPackingOut.RONo;
            Article = garmentSubconPackingOut.Article;
            Unit = new UnitDepartment(garmentSubconPackingOut.UnitId.Value, garmentSubconPackingOut.UnitCode, garmentSubconPackingOut.UnitName);
            PackingOutDate = garmentSubconPackingOut.PackingOutDate;
            PackingOutType = garmentSubconPackingOut.PackingOutType;
            Comodity = new GarmentComodity(garmentSubconPackingOut.ComodityId.Value, garmentSubconPackingOut.ComodityCode, garmentSubconPackingOut.ComodityName);
            ProductOwner = new Buyer(garmentSubconPackingOut.ProductOwnerId.Value, garmentSubconPackingOut.ProductOwnerCode, garmentSubconPackingOut.ProductOwnerName);
            Invoice = garmentSubconPackingOut.Invoice;
            ContractNo = garmentSubconPackingOut.ContractNo;
            Carton = garmentSubconPackingOut.Carton;
            Description = garmentSubconPackingOut.Description;
            CreatedBy = garmentSubconPackingOut.AuditTrail.CreatedBy;
            IsReceived = garmentSubconPackingOut.IsReceived;
        }

        public Guid Id { get; internal set; }
        public string PackingOutNo { get; internal set; }
        public UnitDepartment Unit { get; internal set; }
        public string PackingOutType { get; internal set; }
        public string RONo { get; internal set; }
        public string Article { get; internal set; }
        public GarmentComodity Comodity { get; internal set; }
        public Buyer ProductOwner { get; internal set; }
        public DateTimeOffset PackingOutDate { get; internal set; }
        public string Invoice { get; internal set; }
        public string ContractNo { get; internal set; }
        public double Carton { get; internal set; }
        public string Description { get; internal set; }
        public bool IsReceived { get; private set; }
        public double TotalQuantity { get; set; }
        public double TotalPrice { get; set; }
    }
}
