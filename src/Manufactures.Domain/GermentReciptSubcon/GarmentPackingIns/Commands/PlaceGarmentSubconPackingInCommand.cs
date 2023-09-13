using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GermentReciptSubcon.GarmentPackingIns.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentPackingIns.Commands
{
    public class PlaceGarmentSubconPackingInCommand : ICommand<GarmentSubconPackingIn>
    {
        public UnitDepartment Unit { get;  set; }
        public UnitDepartment UnitFrom { get;  set; }
        public string PackingFrom { get;  set; }
        public string RONo { get;  set; }
        public string Article { get;  set; }
        public GarmentComodity Comodity { get;  set; }
        public DateTimeOffset PackingInDate { get;  set; }
        public DateTimeOffset? DataFromDate { get; set; }
        public List<GarmentSubconPackingInItemValueObject> Items { get; set; }
        public double Price { get; set; }
    }

    public class PlaceGarmentSubconPackingInCommandValidator : AbstractValidator<PlaceGarmentSubconPackingInCommand>
    {
        public PlaceGarmentSubconPackingInCommandValidator()
        {
            RuleFor(r => r).NotNull();
            RuleFor(r => r.Unit.Id).NotEmpty().OverridePropertyName("Unit").When(w => w.Unit != null);
            RuleFor(r => r.UnitFrom).NotNull();
            RuleFor(r => r.UnitFrom.Id).NotEmpty().OverridePropertyName("UnitFrom").When(w => w.UnitFrom != null);
            RuleFor(r => r.RONo).NotNull();
            RuleFor(r => r.PackingInDate).NotNull().GreaterThan(DateTimeOffset.MinValue).WithMessage("Tanggal Packing In Tidak Boleh Kosong");
            RuleFor(r => r.PackingInDate).NotNull().LessThan(DateTimeOffset.Now).WithMessage("Tanggal Packing In Tidak Boleh Lebih dari Hari Ini");
            RuleFor(r => r.PackingInDate).NotNull().GreaterThanOrEqualTo(r => r.DataFromDate.GetValueOrDefault().Date).WithMessage(r => $"Tanggal Packing In Tidak Boleh Kurang dari tanggal {r.DataFromDate.GetValueOrDefault().ToOffset(new TimeSpan(7, 0, 0)).ToString("dd/MM/yyyy", new CultureInfo("id-ID"))}");
            RuleFor(r => r.Comodity).NotNull();
            RuleFor(r => r.Article).NotNull();
            RuleFor(r => r.Price).GreaterThan(0).WithMessage("Tarif komoditi belum ada");
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleFor(r => r.Items.Where(s => s.IsSave == true)).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount").When(s => s.Items != null);
            RuleForEach(r => r.Items).SetValidator(new GarmentSubconPackingInItemValueObjectValidator());
        }
    }

    class GarmentSubconPackingInItemValueObjectValidator : AbstractValidator<GarmentSubconPackingInItemValueObject>
    {
        public GarmentSubconPackingInItemValueObjectValidator()
        {
            RuleFor(r => r.Quantity)
                .GreaterThan(0)
                .WithMessage("'Jumlah' harus lebih dari '0'.")
                .When(w => w.IsSave);

            RuleFor(r => r.Quantity)
                .LessThanOrEqualTo(r => r.DataFromRemainingQuantity)
                .OverridePropertyName("Quantity")
                .WithMessage(x => $"'Jumlah' tidak boleh lebih dari '{x.DataFromRemainingQuantity}'.")
                .When(w => w.IsSave == true);

        }
    }

}
