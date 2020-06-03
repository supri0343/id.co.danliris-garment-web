using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentCuttingOuts.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentCuttingOuts.Commands
{
    public class UpdateGarmentCuttingOutCommand : ICommand<GarmentCuttingOut>
    {
        public Guid Identity { get; private set; }
        public string CutOutNo { get; set; }
        public string CuttingOutType { get; set; }

        public UnitDepartment UnitFrom { get; set; }
        public DateTimeOffset CuttingOutDate { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public UnitDepartment Unit { get; set; }
        public GarmentComodity Comodity { get; set; }
        public List<GarmentCuttingOutItemValueObject> Items { get; set; }

        public void SetIdentity(Guid id)
        {
            Identity = id;
        }

        public class UpdateGarmentCuttingOutCommandValidator : AbstractValidator<UpdateGarmentCuttingOutCommand>
        {
            public UpdateGarmentCuttingOutCommandValidator()
            {
                RuleFor(r => r.UnitFrom).NotNull();
                RuleFor(r => r.UnitFrom.Id).NotEmpty().OverridePropertyName("Unit").When(w => w.Unit != null);

                RuleFor(r => r.Unit).NotNull();
                RuleFor(r => r.Unit.Id).NotEmpty().OverridePropertyName("Unit").When(w => w.Unit != null);
                RuleFor(r => r.Article).NotNull();
                RuleFor(r => r.RONo).NotNull();
                RuleFor(r => r.CuttingOutDate).NotNull().GreaterThan(DateTimeOffset.MinValue);
                RuleFor(r => r.CuttingOutDate).NotNull().LessThan(DateTimeOffset.Now).WithMessage("Tanggal Cutting Out Tidak Boleh Lebih dari Hari Ini");
                RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
                RuleForEach(r => r.Items).SetValidator(new GarmentCuttingOutItemValueObjectValidator());
            }
        }
    }
}