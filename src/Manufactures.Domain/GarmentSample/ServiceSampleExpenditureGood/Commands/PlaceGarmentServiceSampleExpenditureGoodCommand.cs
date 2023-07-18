using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleExpenditureGood.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Domain.GarmentSample.ServiceSampleExpenditureGood.Commands
{
    public class PlaceGarmentServiceSampleExpenditureGoodCommand : ICommand<GarmentServiceSampleExpenditureGood>
    {
        public string ServiceSampleExpenditureGoodNo { get; set; }
        public DateTimeOffset ServiceSampleExpenditureGoodDate { get; set; }
        public bool IsUsed { get; set; }
        public Buyer Buyer { get; set; }
        public int QtyPacking { get; set; }
        public string UomUnit { get; set; }
        public double NettWeight { get; set; }
        public double GrossWeight { get; set; }
        public List<GarmentServiceSampleExpenditureGoodItemValueObject> Items { get; set; }

    }

    public class PlaceGarmentServiceSampleExpenditureGoodCommandValidator : AbstractValidator<PlaceGarmentServiceSampleExpenditureGoodCommand>
    {
        public PlaceGarmentServiceSampleExpenditureGoodCommandValidator()
        {
            //RuleFor(r => r.Unit).NotNull();
            //RuleFor(r => r.Unit.Id).NotEmpty().OverridePropertyName("Unit").When(w => w.Unit != null);
            RuleFor(r => r.ServiceSampleExpenditureGoodDate).NotNull().GreaterThan(DateTimeOffset.MinValue).WithMessage("Tanggal Sample Jasa ExpenditureGood Tidak Boleh Kosong");
            RuleFor(r => r.ServiceSampleExpenditureGoodDate).NotNull().LessThan(DateTimeOffset.Now).WithMessage("Tanggal Sample Jasa ExpenditureGood Tidak Boleh Lebih dari Hari Ini");
            RuleFor(r => r.Buyer).NotNull();
            RuleFor(r => r.Buyer.Id).NotEmpty().OverridePropertyName("Buyer").When(w => w.Buyer != null);
            RuleFor(r => r.NettWeight).NotEmpty().WithMessage("Nett Weight tidak boleh kosong!").OverridePropertyName("Nett Weight");
            RuleFor(r => r.GrossWeight).NotEmpty().WithMessage("Gross Weight tidak boleh kosong!").OverridePropertyName("Gross Weight");
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleFor(r => r.Items).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount");
            RuleForEach(r => r.Items).SetValidator(new GarmentServiceSampleExpenditureGoodItemValueObjectValidator()).When(s => s.Items != null);
        }
    }

    public class GarmentServiceSampleExpenditureGoodItemValueObjectValidator : AbstractValidator<GarmentServiceSampleExpenditureGoodItemValueObject>
    {
        public GarmentServiceSampleExpenditureGoodItemValueObjectValidator()
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
            //RuleForEach(r => r.Details).SetValidator(new GarmentServiceSampleExpenditureGoodDetailValueObjectValidator());
        }
    }




}
