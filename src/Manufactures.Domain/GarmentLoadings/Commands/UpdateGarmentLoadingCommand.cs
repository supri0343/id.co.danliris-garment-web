using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentLoadings.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentLoadings.Commands
{
    public class UpdateGarmentLoadingCommand : ICommand<GarmentLoading>
    {
        public Guid Identity { get; private set; }
        public string LoadingNo { get;  set; }
        public Guid SewingDOId { get;  set; }
        public string SewingDONo { get;  set; }
        public UnitDepartment UnitFrom { get;  set; }
        public UnitDepartment Unit { get;  set; }
        public string RONo { get;  set; }
        public string Article { get;  set; }
        public GarmentComodity Comodity { get;  set; }
        public DateTimeOffset LoadingDate { get;  set; }

        public List<GarmentLoadingItemValueObject> Items { get;  set; }

        public void SetIdentity(Guid id)
        {
            Identity = id;
        }
    }

    public class UpdateGarmentLoadingCommandValidator : AbstractValidator<UpdateGarmentLoadingCommand>
    {
        public UpdateGarmentLoadingCommandValidator()
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
}
