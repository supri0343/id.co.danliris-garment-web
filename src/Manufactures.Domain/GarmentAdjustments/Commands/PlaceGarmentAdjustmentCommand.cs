using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using Manufactures.Domain.GarmentAdjustments.ValueObjects;
using System.Text;
using FluentValidation;
using System.Linq;

namespace Manufactures.Domain.GarmentAdjustments.Commands
{
    public class PlaceGarmentAdjustmentCommand : ICommand<GarmentAdjustment>
    {
        public string AdjustmentNo { get; set; }
        public string AdjustmentType { get; set; }
        public UnitDepartment Unit { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public GarmentComodity Comodity { get; set; }
        public DateTimeOffset AdjustmentDate { get; set; }
        public double Price { get; set; }
        public string AdjustmentDesc { get; set; }

        public List<GarmentAdjustmentItemValueObject> Items { get; set; }
    }

    public class PlaceGarmentAdjustmentCommandValidator : AbstractValidator<PlaceGarmentAdjustmentCommand>
    {
        public PlaceGarmentAdjustmentCommandValidator()
        {
            RuleFor(r => r.Unit).NotNull();
            RuleFor(r => r.Unit.Id).NotEmpty().OverridePropertyName("Unit").When(w => w.Unit != null);
            RuleFor(r => r.RONo).NotNull();
            RuleFor(r => r.AdjustmentDate).NotNull().GreaterThan(DateTimeOffset.MinValue).WithMessage("Tanggal Adjustment Tidak Boleh Kosong");
            RuleFor(r => r.AdjustmentDate).NotNull().LessThan(DateTimeOffset.Now).WithMessage("Tanggal Adjustment Tidak Boleh Lebih dari Hari Ini");
            RuleFor(r => r.Comodity).NotNull();

            RuleFor(r => r.Price).GreaterThan(0).WithMessage("Tarif komoditi belum ada");

            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleFor(r => r.Items).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount");
            RuleFor(r => r.Items.Where(s => s.IsSave == true)).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount").When(s => s.Items != null);
            RuleForEach(r => r.Items).SetValidator(new GarmentAdjustmentItemValueObjectValidator());
        }
    }

    class GarmentAdjustmentItemValueObjectValidator : AbstractValidator<GarmentAdjustmentItemValueObject>
    {
        public GarmentAdjustmentItemValueObjectValidator()
        {
            RuleFor(r => r.Quantity)
                .GreaterThan(0)
                .WithMessage("'Jumlah' harus lebih dari '0'.")
                .When(w => w.IsSave);

            RuleFor(r => r.Quantity)
                .LessThanOrEqualTo(r => r.RemainingQuantity)
                .OverridePropertyName("Quantity")
                .WithMessage(x => $"'Jumlah' tidak boleh lebih dari '{x.RemainingQuantity}'.")
                .When(w => w.IsSave == true);
        }
    }
}
