using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentLoadings.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentLoadings.Commands
{
    public class PlaceGarmentLoadingCommand : ICommand<GarmentLoading>
    {
        public string LoadingNo { get; internal set; }
        public Guid SewingDOId { get; internal set; }
        public string SewingDONo { get; internal set; }
        public UnitDepartment UnitFrom { get; internal set; }
        public UnitDepartment Unit { get; internal set; }
        public string RONo { get; internal set; }
        public string Article { get; internal set; }
        public string Comodity { get; internal set; }
        public DateTimeOffset LoadingDate { get; internal set; }

        public List<GarmentLoadingItemValueObject> Items { get; internal set; }
    }

    public class PlaceGarmentLoadingCommandValidator : AbstractValidator<PlaceGarmentLoadingCommand>
    {
        public PlaceGarmentLoadingCommandValidator()
        {
            RuleFor(r => r.Unit).NotNull();
            RuleFor(r => r.Unit.Id).NotEmpty().OverridePropertyName("Unit").When(w => w.Unit != null);
            RuleFor(r => r.SewingDOId).NotNull();
            RuleFor(r => r.RONo).NotNull();
            RuleFor(r => r.LoadingDate).NotNull().GreaterThan(DateTimeOffset.MinValue);
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleForEach(r => r.Items).SetValidator(new GarmentLoadingItemValueObjectValidator());
        }
    }

    class GarmentLoadingItemValueObjectValidator : AbstractValidator<GarmentLoadingItemValueObject>
    {
        public GarmentLoadingItemValueObjectValidator()
        {
            RuleFor(r => r.Quantity)
                .GreaterThan(0)
                .WithMessage("'Jumlah' harus lebih dari '0'.");
                //.When(w => w.IsSave);

        }
    }
}
