using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSewingIns.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSewingIns.Commands
{
    public class PlaceGarmentSewingInCommand : ICommand<GarmentSewingIn>
    {
        public string SewingInNo { get; set; }
        public Guid LoadingId { get; set; }
        public string LoadingNo { get; set; }
        public UnitDepartment UnitFrom { get; set; }
        public UnitDepartment Unit { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public GarmentComodity Comodity { get; set; }
        public DateTimeOffset? SewingInDate { get; set; }

        public List<GarmentSewingInItemValueObject> Items { get; set; }
    }

    public class PlaceGarmentSewingInCommandValidator : AbstractValidator<PlaceGarmentSewingInCommand>
    {
        public PlaceGarmentSewingInCommandValidator()
        {
            RuleFor(r => r.UnitFrom).NotNull();
            RuleFor(r => r.UnitFrom.Id).NotEmpty().OverridePropertyName("UnitFrom").When(w => w.Unit != null);

            RuleFor(r => r.Unit).NotNull();
            RuleFor(r => r.Unit.Id).NotEmpty().OverridePropertyName("Unit").When(w => w.Unit != null);

            RuleFor(r => r.Comodity).NotNull();
            RuleFor(r => r.Comodity.Id).NotEmpty().OverridePropertyName("Comodity").When(w => w.Comodity != null);

            RuleFor(r => r.RONo).NotNull();
            RuleFor(r => r.SewingInDate).NotNull().GreaterThan(DateTimeOffset.MinValue);
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleFor(r => r.Items).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount");
            //RuleFor(r => r.Items.Where(s => s.IsSave == true)).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount").When(s => s.Items != null);
            RuleForEach(r => r.Items).SetValidator(new GarmentSewingInItemValueObjectValidator());
        }
    }

    class GarmentSewingInItemValueObjectValidator : AbstractValidator<GarmentSewingInItemValueObject>
    {
        public GarmentSewingInItemValueObjectValidator()
        {
        }
    }
}