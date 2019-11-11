using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentComodityPrices.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentComodityPrices.Commands
{
    public class PlaceGarmentComodityPriceCommand : ICommand<List<GarmentComodityPrice>>
    {
        public UnitDepartment Unit { get; set; }
        public DateTimeOffset Date { get; set; }

        public List<GarmentComodityPriceItemValueObject> Items { get; set; }
    }

    public class PlaceGarmentComodityPriceCommandValidator : AbstractValidator<PlaceGarmentComodityPriceCommand>
    {
        public PlaceGarmentComodityPriceCommandValidator()
        {
            RuleFor(r => r.Unit).NotNull();
            RuleFor(r => r.Unit.Id).NotEmpty().OverridePropertyName("Unit").When(w => w.Unit != null);

            RuleFor(r => r.Date).NotNull().GreaterThan(DateTimeOffset.MinValue);
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleForEach(r => r.Items).SetValidator(new GarmentComodityPriceItemValueObjectValidator());
        }
    }

    class GarmentComodityPriceItemValueObjectValidator : AbstractValidator<GarmentComodityPriceItemValueObject>
    {
        public GarmentComodityPriceItemValueObjectValidator()
        {
            RuleFor(r => r.Comodity).NotEmpty().OverridePropertyName("Comodity");
            RuleFor(r => r.Price)
                 .GreaterThan(0)
                 .WithMessage("'Harga' harus lebih dari '0'.");
        }
    }
}
