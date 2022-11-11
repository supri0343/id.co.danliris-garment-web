using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.SubconReprocess.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.SubconReprocess.Commands
{
    public class UpdateGarmentSubconReprocessCommand : ICommand<GarmentSubconReprocess>
    {
        public Guid Identity { get; set; }
        public string ReprocessNo { get; set; }
        public string ReprocessType { get; set; }
        public DateTimeOffset Date { get; set; }
        public virtual List<GarmentSubconReprocessItemValueObject> Items { get; set; }
        public void SetIdentity(Guid id)
        {
            Identity = id;
        }
    }
    public class UpdateGarmentSubconReprocessCommandValidator : AbstractValidator<UpdateGarmentSubconReprocessCommand>
    {
        public UpdateGarmentSubconReprocessCommandValidator()
        {
            RuleFor(r => r.Date).NotNull().GreaterThan(DateTimeOffset.MinValue);
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleFor(r => r.Items).NotEmpty().WithMessage("Data Belum Ada yang dipilih").OverridePropertyName("ItemsCount").When(s => s.Items != null);

            RuleForEach(r => r.Items).SetValidator(new GarmentSubconReprocessItemValueObjectValidator());
        }
    }
}
