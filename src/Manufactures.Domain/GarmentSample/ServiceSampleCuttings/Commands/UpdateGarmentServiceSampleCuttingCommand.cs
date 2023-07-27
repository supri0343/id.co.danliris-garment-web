using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSample.ServiceSampleCuttings.Commands
{
    public class UpdateGarmentServiceSampleCuttingCommand : ICommand<GarmentServiceSampleCutting>
    {
        public Guid Identity { get; private set; }
        public UnitDepartment Unit { get; set; }
        public DateTimeOffset? SubconDate { get; set; }
        public string SubconNo { get; set; }
        public string SubconType { get; set; }

        public bool IsUsed { get; set; }
        public Buyer Buyer { get; set; }
        public Uom Uom { get; set; }
        public int QtyPacking { get; set; }
        public double NettWeight { get; set; }
        public double GrossWeight { get; set; }
        public string Remark { get; set; }
        public List<GarmentServiceSampleCuttingItemValueObject> Items { get; set; }

        public void SetIdentity(Guid id)
        {
            Identity = id;
        }
    }

    public class UpdateGarmentServiceSampleCuttingCommandValidator : AbstractValidator<UpdateGarmentServiceSampleCuttingCommand>
    {
        public UpdateGarmentServiceSampleCuttingCommandValidator()
        {
            RuleFor(r => r.Unit).NotNull();
            RuleFor(r => r.Unit.Id).NotEmpty().OverridePropertyName("Unit").When(w => w.Unit != null);
            RuleFor(r => r.Buyer).NotNull();
            RuleFor(r => r.Buyer.Id).NotEmpty().OverridePropertyName("Buyer").When(w => w.Buyer != null);
            RuleFor(r => r.SubconDate).NotNull().GreaterThan(DateTimeOffset.MinValue);
            RuleFor(r => r.Uom).NotNull();
            RuleFor(r => r.Uom.Id).NotEmpty().OverridePropertyName("Uom").When(w => w.Uom != null);
            RuleFor(r => r.QtyPacking).NotEmpty().WithMessage("Jumlah Packing tidak boleh kosong!").OverridePropertyName("QtyPacking");
            RuleFor(r => r.NettWeight).NotEmpty().WithMessage("Nett Weight tidak boleh kosong!").OverridePropertyName("Nett Weight");
            RuleFor(r => r.GrossWeight).NotEmpty().WithMessage("Gross Weight tidak boleh kosong!").OverridePropertyName("Gross Weight");
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleFor(r => r.Items).NotEmpty().WithMessage("Data Belum Ada yang dipilih").OverridePropertyName("ItemsCount").When(s => s.Items != null);
            RuleForEach(r => r.Items).SetValidator(new GarmentServiceSampleCuttingItemValueObjectValidator());
        }
    }

}
