using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconExpenditureGood.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconExpenditureGood.Commands
{
    public class PlaceGarmentServiceSubconExpenditureGoodCommand : ICommand<GarmentServiceSubconExpenditureGood>
    {
        public string ServiceSubconExpenditureGoodNo { get; set; }
        public DateTimeOffset ServiceSubconExpenditureGoodDate { get; set; }
        public bool IsUsed { get; set; }
        public Buyer Buyer { get; set; }
        public int QtyPacking { get; set; }
        public string UomUnit { get; set; }
        public double NettWeight { get; set; }
        public double GrossWeight { get; set; }
        public List<GarmentServiceSubconExpenditureGoodItemValueObject> Items { get; set; }

    }

    public class PlaceGarmentServiceSubconExpenditureGoodCommandValidator : AbstractValidator<PlaceGarmentServiceSubconExpenditureGoodCommand>
    {
        public PlaceGarmentServiceSubconExpenditureGoodCommandValidator()
        {
            //RuleFor(r => r.Unit).NotNull();
            //RuleFor(r => r.Unit.Id).NotEmpty().OverridePropertyName("Unit").When(w => w.Unit != null);
            RuleFor(r => r.ServiceSubconExpenditureGoodDate).NotNull().GreaterThan(DateTimeOffset.MinValue).WithMessage("Tanggal Subcon Jasa ExpenditureGood Tidak Boleh Kosong");
            RuleFor(r => r.ServiceSubconExpenditureGoodDate).NotNull().LessThan(DateTimeOffset.Now).WithMessage("Tanggal Subcon Jasa ExpenditureGood Tidak Boleh Lebih dari Hari Ini");
            RuleFor(r => r.Buyer).NotNull();
            RuleFor(r => r.Buyer.Id).NotEmpty().OverridePropertyName("Buyer").When(w => w.Buyer != null);
            RuleFor(r => r.NettWeight).NotEmpty().WithMessage("Nett Weight tidak boleh kosong!").OverridePropertyName("Nett Weight");
            RuleFor(r => r.GrossWeight).NotEmpty().WithMessage("Gross Weight tidak boleh kosong!").OverridePropertyName("Gross Weight");
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleFor(r => r.Items).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount");
            RuleForEach(r => r.Items).SetValidator(new GarmentServiceSubconExpenditureGoodItemValueObjectValidator()).When(s => s.Items != null);
        }
    }

    public class GarmentServiceSubconExpenditureGoodItemValueObjectValidator : AbstractValidator<GarmentServiceSubconExpenditureGoodItemValueObject>
    {
        public GarmentServiceSubconExpenditureGoodItemValueObjectValidator()
        {
            RuleFor(r => r.Article).NotNull();
            RuleFor(r => r.RONo).NotNull();
            RuleFor(r => r.Quantity)
            .GreaterThan(0)
            .WithMessage("'Jumlah' harus lebih dari '0'.");
            RuleFor(r => r.Quantity)
               .LessThanOrEqualTo(r => r.StockQuantity).When(s => s.StockQuantity > 0)
               .WithMessage(x => $"'Jumlah' tidak boleh lebih dari '{x.StockQuantity}'.");
            //RuleFor(r => r.Details).NotEmpty().OverridePropertyName("Details");
            //RuleFor(r => r.Details).NotEmpty().WithMessage("Details Tidak Boleh Kosong").OverridePropertyName("DetailsCount");
            //RuleFor(r => r.Details.Where(s => s.IsSave == true)).NotEmpty().WithMessage("Details Tidak Boleh Kosong").OverridePropertyName("DetailsCount").When(s => s.Details != null);
            //RuleForEach(r => r.Details).SetValidator(new GarmentServiceSubconExpenditureGoodDetailValueObjectValidator());
        }
    }




}
