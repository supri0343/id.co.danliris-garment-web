using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentAvalComponents.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.GarmentAvalComponents.Commands
{
    public class PlaceGarmentAvalComponentCommand : ICommand<GarmentAvalComponent>
    {
        public UnitDepartment Unit { get; set; }
        public string AvalComponentType { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public GarmentComodity Comodity { get; set; }
        public DateTimeOffset? Date { get; set; }

        public List<PlaceGarmentAvalComponentItemValueObject> Items { get; set; }
    }

    public class PlaceGarmentAvalComponentCommandValidator : AbstractValidator<PlaceGarmentAvalComponentCommand>
    {
        public PlaceGarmentAvalComponentCommandValidator()
        {
            RuleFor(r => r.Unit).NotNull();
            RuleFor(r => r.Unit.Id).NotEmpty().OverridePropertyName("Unit").When(w => w.Unit != null);
            RuleFor(r => r.Unit.Code).NotEmpty().OverridePropertyName("Unit").When(w => w.Unit != null && w.Unit.Id > 0);

            RuleFor(r => r.AvalComponentType).NotNull();
            RuleFor(r => r.RONo).NotNull();

            RuleFor(r => r.Comodity).NotNull().When(w => w.AvalComponentType == "SEWING");
            RuleFor(r => r.Comodity.Id).NotEmpty().OverridePropertyName("Comodity").When(w => w.AvalComponentType == "SEWING" && w.Comodity != null);

            RuleFor(r => r.Date).NotEmpty();

            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");

            RuleForEach(r => r.Items).SetValidator(command => new PlaceGarmentAvalComponentItemValidator(command));
        }
    }

    class PlaceGarmentAvalComponentItemValidator : AbstractValidator<PlaceGarmentAvalComponentItemValueObject>
    {
        public PlaceGarmentAvalComponentItemValidator(PlaceGarmentAvalComponentCommand placeGarmentAvalComponentCommand)
        {
            RuleFor(r => r.Product).NotNull();

            RuleFor(r => r.Quantity).LessThanOrEqualTo(r => r.SourceQuantity);
        }
    }
}
