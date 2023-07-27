using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleExpenditureGood.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Domain.GarmentSample.ServiceSampleExpenditureGood.Commands
{
    public class UpdateGarmentServiceSampleExpenditureGoodCommand : ICommand<GarmentServiceSampleExpenditureGood>
    {
        public Guid Identity { get; private set; }
        public string ServiceSubconExpenditureGoodNo { get; set; }
        public DateTimeOffset? ServiceSubconExpenditureGoodDate { get; set; }
        public bool IsUsed { get; set; }
        public Buyer Buyer { get; set; }
        public int QtyPacking { get; set; }
        public string UomUnit { get; set; }
        public double NettWeight { get; set; }
        public double GrossWeight { get; set; }
        public List<GarmentServiceSampleExpenditureGoodItemValueObject> Items { get; set; }

        public void SetIdentity(Guid id)
        {
            Identity = id;
        }
    }

    public class UpdateGarmentServiceSampleExpenditureGoodCommandValidator : AbstractValidator<UpdateGarmentServiceSampleExpenditureGoodCommand>
    {
        public UpdateGarmentServiceSampleExpenditureGoodCommandValidator()
        {
            //RuleFor(r => r.Unit).NotNull();
            //RuleFor(r => r.Unit.Id).NotEmpty().OverridePropertyName("Unit").When(w => w.Unit != null);
            RuleFor(r => r.ServiceSubconExpenditureGoodDate).NotNull().GreaterThan(DateTimeOffset.MinValue).WithMessage("Tanggal Sample Jasa ExpenditureGood Tidak Boleh Kosong");
            RuleFor(r => r.ServiceSubconExpenditureGoodDate).NotNull().LessThan(DateTimeOffset.Now).WithMessage("Tanggal Sample Jasa ExpenditureGood Tidak Boleh Lebih dari Hari Ini");
            RuleFor(r => r.Buyer).NotNull();
            RuleFor(r => r.Buyer.Id).NotEmpty().OverridePropertyName("Buyer").When(w => w.Buyer != null);
            RuleFor(r => r.NettWeight).NotEmpty().WithMessage("Nett Weight tidak boleh kosong!").OverridePropertyName("Nett Weight");
            RuleFor(r => r.GrossWeight).NotEmpty().WithMessage("Gross Weight tidak boleh kosong!").OverridePropertyName("Gross Weight");
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleFor(r => r.Items).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount");
            RuleForEach(r => r.Items).SetValidator(new GarmentServiceSampleExpenditureGoodItemValueObjectValidator());
        }
    }


}

