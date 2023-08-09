using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels.Commands
{
    public class UpdateGarmentServiceSampleShrinkagePanelCommand : ICommand<GarmentServiceSampleShrinkagePanel>
    {
        public Guid Identity { get; private set; }
        public string ServiceSubconShrinkagePanelNo { get; set; }
        public DateTimeOffset? ServiceSubconShrinkagePanelDate { get; set; }
        public string Remark { get; set; }
        public bool IsUsed { get; set; }
        public int QtyPacking { get; set; }
        public string UomUnit { get; set; }
        public double NettWeight { get; set; }
        public double GrossWeight { get; set; }
        public List<GarmentServiceSampleShrinkagePanelItemValueObject> Items { get; set; }

        public void SetIdentity(Guid id)
        {
            Identity = id;
        }
    }

    public class UpdateGarmentServiceSampleShrinkagePanelCommandValidator : AbstractValidator<UpdateGarmentServiceSampleShrinkagePanelCommand>
    {
        public UpdateGarmentServiceSampleShrinkagePanelCommandValidator()
        {
            RuleFor(r => r.ServiceSubconShrinkagePanelDate).NotNull().GreaterThan(DateTimeOffset.MinValue).WithMessage("Tanggal Sample Jasa Shrinkage Panel Tidak Boleh Kosong");
            //RuleFor(r => r.ServiceSampleSewingDate).NotNull().LessThan(DateTimeOffset.Now).WithMessage("Tanggal Sample Jasa Sewing Tidak Boleh Lebih dari Hari Ini");
            RuleFor(r => r.NettWeight).NotEmpty().WithMessage("Nett Weight tidak boleh kosong!").OverridePropertyName("Nett Weight");
            RuleFor(r => r.GrossWeight).NotEmpty().WithMessage("Gross Weight tidak boleh kosong!").OverridePropertyName("Gross Weight");
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleFor(r => r.Items).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount");
            RuleForEach(r => r.Items).SetValidator(new GarmentServiceSampleShrinkagePanelItemValueObjectValidator());
        }
    }
}
