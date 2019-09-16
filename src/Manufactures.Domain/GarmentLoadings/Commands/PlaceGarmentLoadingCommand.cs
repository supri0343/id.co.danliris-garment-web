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
        public string LoadingNo { get; set; }
        public Guid SewingDOId { get; set; }
        public string SewingDONo { get; set; }
        public UnitDepartment UnitFrom { get; set; }
        public UnitDepartment Unit { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public GarmentComodity Comodity { get; set; }
        public DateTimeOffset LoadingDate { get; set; }

        public List<GarmentLoadingItemValueObject> Items { get; set; }
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
            RuleFor(r => r.Comodity).NotNull();
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
                .WithMessage("'Jumlah' harus lebih dari '0'.")
                .When(w => w.IsSave);

            RuleFor(r => r.Quantity)
                .LessThanOrEqualTo(r => r.SewingDORemainingQuantity)
                .OverridePropertyName("Quantity")
                .WithMessage(x => $"'Jumlah' tidak boleh lebih dari '{x.SewingDORemainingQuantity}'.")
                .When(w => w.IsSave == true);
        }
    }
}
