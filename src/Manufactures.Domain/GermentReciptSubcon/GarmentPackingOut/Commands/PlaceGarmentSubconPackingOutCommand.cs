using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentPackingOut.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentPackingOut.Commands
{
    public class PlaceGarmentSubconPackingOutCommand : ICommand<GarmentSubconPackingOut>
    {
        public string PackingOutNo { get;  set; }
        public UnitDepartment Unit { get;  set; }
        public string PackingOutType { get;  set; }
        public string RONo { get;  set; }
        public string Article { get;  set; }
        public GarmentComodity Comodity { get;  set; }
        public Buyer ProductOwner { get;  set; }
        public DateTimeOffset PackingOutDate { get;  set; }
        public string Invoice { get;  set; }
        public int PackingListId { get; set; }
        public string ContractNo { get;  set; }
        public double Carton { get;  set; }
        public string Description { get;  set; }
        public List<GarmentSubconPackingOutItemValueObject> Items { get;  set; }
        public double Price { get; set; }
        public bool IsReceived { get; set; }
		public int InvoiceId { get; set; }
		public Guid PackingInId { get; set; }
		public double TotalQty { get; set; }
    }

    public class PlaceGarmentPackingOutCommandValidator : AbstractValidator<PlaceGarmentSubconPackingOutCommand>
    {
        public PlaceGarmentPackingOutCommandValidator()
        {
            RuleFor(r => r.Unit).NotNull();
            RuleFor(r => r.Unit.Id).NotEmpty().OverridePropertyName("Unit").When(w => w.Unit != null);
            RuleFor(r => r.RONo).NotNull();
            RuleFor(r => r.PackingOutDate).NotNull().GreaterThan(DateTimeOffset.MinValue).WithMessage("Tanggal Tidak Boleh Kosong");
            RuleFor(r => r.PackingOutDate).NotNull().LessThan(DateTimeOffset.Now).WithMessage("Tanggal Tidak Boleh Lebih dari Hari Ini");
            RuleFor(r => r.Comodity).NotNull();
            RuleFor(r => r.Carton)
                .GreaterThanOrEqualTo(0)
                .WithMessage("'Karton' harus lebih dari atau sama dengan '0'.");
            RuleFor(r => r.Article).NotNull();
            RuleFor(r => r.Price).GreaterThan(0).WithMessage("Tarif komoditi belum ada");
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleFor(r => r.Items).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount");
            RuleFor(r => r.Items.Where(s => s.isSave == true)).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount").When(s => s.Items != null);
            RuleForEach(r => r.Items).SetValidator(new GarmentPackingOutItemValueObjectValidator());
        }
    }

    class GarmentPackingOutItemValueObjectValidator : AbstractValidator<GarmentSubconPackingOutItemValueObject>
    {
        public GarmentPackingOutItemValueObjectValidator()
        {
            RuleFor(r => r.Quantity)
                .GreaterThan(0)
                .WithMessage("'Jumlah' harus lebih dari '0'.")
                .When(a=>a.isSave);

            RuleFor(r => r.Quantity)
               .LessThanOrEqualTo(r => r.StockQuantity)
               .WithMessage(x => $"'Jumlah' tidak boleh lebih dari '{x.StockQuantity}'.").When(w => w.isSave == true);

        }
    }
}
