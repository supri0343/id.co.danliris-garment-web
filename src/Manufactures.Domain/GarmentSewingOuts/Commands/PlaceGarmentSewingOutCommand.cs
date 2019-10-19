using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSewingOuts.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSewingOuts.Commands
{
    public class PlaceGarmentSewingOutCommand : ICommand<GarmentSewingOut>
    {
        public string SewingOutNo { get;  set; }
        public Buyer Buyer { get;  set; }
        public UnitDepartment Unit { get;  set; }
        public string SewingTo { get;  set; }
        public UnitDepartment UnitTo { get;  set; }
        public string RONo { get;  set; }
        public string Article { get;  set; }
        public GarmentComodity Comodity { get;  set; }
        public DateTimeOffset SewingOutDate { get;  set; }
        public bool IsDifferentSize { get;  set; }
        public List<GarmentSewingOutItemValueObject> Items { get; set; }
        public bool IsSave { get; set; }
    }

    public class PlaceGarmentSewingOutCommandValidator : AbstractValidator<PlaceGarmentSewingOutCommand>
    {
        public PlaceGarmentSewingOutCommandValidator()
        {
            RuleFor(r => r.Unit).NotNull();
            RuleFor(r => r.Unit.Id).NotEmpty().OverridePropertyName("Unit").When(w => w.Unit != null);

            RuleFor(r => r.RONo).NotNull();
            RuleFor(r => r.SewingOutDate).NotNull().GreaterThan(DateTimeOffset.MinValue);
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleForEach(r => r.Items).SetValidator(new GarmentSewingOutItemValueObjectValidator());
        }
    }

    class GarmentSewingOutItemValueObjectValidator : AbstractValidator<GarmentSewingOutItemValueObject>
    {
        public GarmentSewingOutItemValueObjectValidator()
        {
            RuleFor(r => r.Details).NotEmpty().OverridePropertyName("Detail");
            RuleForEach(r => r.Details).SetValidator(new GarmentSewingOutDetailValueObjectValidator());
        }
    }

    class GarmentSewingOutDetailValueObjectValidator : AbstractValidator<GarmentSewingOutDetailValueObject>
    {
        public GarmentSewingOutDetailValueObjectValidator()
        {

            RuleFor(r => r.Size).NotNull();
            RuleFor(r => r.Size.Id).NotEmpty().OverridePropertyName("Size").When(w => w.Size != null);
            RuleFor(r => r.Quantity)
                .GreaterThan(0)
                .WithMessage("'Jumlah Potong' harus lebih dari '0'.");

        }
    }
}
