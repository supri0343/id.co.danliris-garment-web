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
