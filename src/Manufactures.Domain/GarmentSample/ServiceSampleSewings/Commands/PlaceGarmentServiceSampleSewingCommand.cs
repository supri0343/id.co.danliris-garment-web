using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Domain.GarmentSample.ServiceSampleSewings.Commands
{
    public class PlaceGarmentServiceSampleSewingCommand : ICommand<GarmentServiceSampleSewing>
    {
        public string ServiceSampleSewingNo { get; set; }
        public DateTimeOffset? ServiceSampleSewingDate { get; set; }
        public bool IsUsed { get; set; }
        public List<GarmentServiceSampleSewingItemValueObject> Items { get; set; }
        public bool IsSave { get; set; }
        public Buyer Buyer { get; set; }
        public int QtyPacking { get; set; }
        public string UomUnit { get; set; }
        public double NettWeight { get; set; }
        public double GrossWeight { get; set; }

    }

    public class PlaceGarmentServiceSampleSewingCommandValidator : AbstractValidator<PlaceGarmentServiceSampleSewingCommand>
    {
        public PlaceGarmentServiceSampleSewingCommandValidator()
        {
            //RuleFor(r => r.Unit).NotNull();
            //RuleFor(r => r.Unit.Id).NotEmpty().OverridePropertyName("Unit").When(w => w.Unit != null);
            RuleFor(r => r.ServiceSampleSewingDate).NotNull().GreaterThan(DateTimeOffset.MinValue).WithMessage("Tanggal Sample Jasa Sewing Tidak Boleh Kosong");
            RuleFor(r => r.ServiceSampleSewingDate).NotNull().LessThan(DateTimeOffset.Now).WithMessage("Tanggal Sample Jasa Sewing Tidak Boleh Lebih dari Hari Ini");
            RuleFor(r => r.Buyer).NotNull();
            RuleFor(r => r.Buyer.Id).NotEmpty().OverridePropertyName("Buyer").When(w => w.Buyer != null);
            //RuleFor(r => r.NettWeight).NotEmpty().WithMessage("Nett Weight tidak boleh kosong!").OverridePropertyName("Nett Weight");
            //RuleFor(r => r.GrossWeight).NotEmpty().WithMessage("Gross Weight tidak boleh kosong!").OverridePropertyName("Gross Weight");
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleFor(r => r.Items).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount");
            RuleForEach(r => r.Items).SetValidator(new GarmentServiceSampleSewingItemValueObjectValidator());
        }
    }

    class GarmentServiceSampleSewingItemValueObjectValidator : AbstractValidator<GarmentServiceSampleSewingItemValueObject>
    {
        public GarmentServiceSampleSewingItemValueObjectValidator()
        {
            RuleFor(r => r.Article).NotNull();
            RuleFor(r => r.RONo).NotNull();
            RuleFor(r => r.Details).NotEmpty().OverridePropertyName("Details");
            RuleFor(r => r.Details).NotEmpty().WithMessage("Details Tidak Boleh Kosong").OverridePropertyName("DetailsCount");
            RuleFor(r => r.Details.Where(s => s.IsSave == true)).NotEmpty().WithMessage("Details Tidak Boleh Kosong").OverridePropertyName("DetailsCount").When(s => s.Details != null);
            RuleForEach(r => r.Details).SetValidator(new GarmentServiceSampleSewingDetailValueObjectValidator());
        }
    }

    class GarmentServiceSampleSewingDetailValueObjectValidator : AbstractValidator<GarmentServiceSampleSewingDetailValueObject>
    {
        public GarmentServiceSampleSewingDetailValueObjectValidator()
        {
            RuleFor(r => r.Quantity)
                .GreaterThan(0)
                .WithMessage("'Jumlah' harus lebih dari '0'.").When(r => r.IsSave == true);
            RuleFor(r => r.Quantity)
               .LessThanOrEqualTo(r => r.SewingInQuantity)
               .WithMessage(x => $"'Jumlah' tidak boleh lebih dari '{x.SewingInQuantity}'.").When(w => w.IsSave == true);
        }
    }
}
