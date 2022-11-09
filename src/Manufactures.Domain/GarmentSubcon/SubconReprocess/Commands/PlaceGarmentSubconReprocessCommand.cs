using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.SubconReprocess.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.SubconReprocess.Commands
{
    public class PlaceGarmentSubconReprocessCommand : ICommand<GarmentSubconReprocess>
    {
        public string ReprocessNo { get; set; }
        public string ReprocessType { get; set; }
        public DateTimeOffset Date { get; set; }
        public virtual List<GarmentSubconReprocessItemValueObject> Items { get; set; }
    }
    public class PlaceGarmentServiceSubconCuttingCommandValidator : AbstractValidator<PlaceGarmentSubconReprocessCommand>
    {
        public PlaceGarmentServiceSubconCuttingCommandValidator()
        {
            RuleFor(r => r.Date).NotNull().GreaterThan(DateTimeOffset.MinValue);
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleFor(r => r.Items).NotEmpty().WithMessage("Data Belum Ada yang dipilih").OverridePropertyName("ItemsCount").When(s => s.Items != null);
            
            RuleForEach(r => r.Items).SetValidator(new GarmentSubconReprocessItemValueObjectValidator());
        }
    }

    class GarmentSubconReprocessItemValueObjectValidator : AbstractValidator<GarmentSubconReprocessItemValueObject>
    {
        public GarmentSubconReprocessItemValueObjectValidator()
        {
            RuleFor(r => r.ServiceSubconCuttingNo).NotNull().When(a => a.Type == "SUBCON JASA KOMPONEN");
            RuleFor(r => r.ServiceSubconSewingNo).NotNull().When(a => a.Type == "SUBCON JASA GARMENT WASH");
            RuleFor(r => r.Article).NotNull();
            RuleFor(r => r.RONo).NotNull();
            RuleFor(r => r.Comodity).NotNull();

            RuleFor(r => r.Comodity.Id).NotEmpty().OverridePropertyName("Comodity").When(w => w.Comodity != null);

            RuleFor(r => r.Details).NotEmpty().WithMessage("Detail data harus diisi").OverridePropertyName("DetailsCount").When(s => s.Details != null);
            RuleFor(r => r.Details).NotEmpty().OverridePropertyName("Detail");
            RuleForEach(r => r.Details).SetValidator(new GarmentSubconReprocessDetailValueObjectValidator());
        }
    }

    class GarmentSubconReprocessDetailValueObjectValidator : AbstractValidator<GarmentSubconReprocessDetailValueObject>
    {
        public GarmentSubconReprocessDetailValueObjectValidator()
        {
            RuleFor(r => r.ReprocessQuantity)
                .GreaterThan(0)
                .WithMessage("'Jumlah reproses' harus lebih dari '0'.");

            RuleFor(r => r.ReprocessQuantity)
                .LessThanOrEqualTo(r => r.RemQty)
                .WithMessage(x => $"'Jumlah reproses' tidak boleh lebih dari '{x.RemQty}'.");

        }
    }
}
