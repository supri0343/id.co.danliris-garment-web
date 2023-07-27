using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels.Commands
{
    public class PlaceGarmentServiceSampleShrinkagePanelCommand : ICommand<GarmentServiceSampleShrinkagePanel>
    {
        public string ServiceSubconShrinkagePanelNo { get; set; }
        public DateTimeOffset? ServiceSubconShrinkagePanelDate { get; set; }
        public string Remark { get; set; }
        public bool IsUsed { get; set; }
        public int QtyPacking { get; set; }
        public string UomUnit { get; set; }
        public double NettWeight { get; set; }
        public double GrossWeight { get; set; }
        public List<GarmentServiceSampleShrinkagePanelItemValueObject> Items { get; set; }
        public bool IsSave { get; set; }
    }
    public class PlaceGarmentServiceSampleShrinkagePanelCommandValidator : AbstractValidator<PlaceGarmentServiceSampleShrinkagePanelCommand>
    {
        public PlaceGarmentServiceSampleShrinkagePanelCommandValidator()
        {
            RuleFor(r => r.ServiceSubconShrinkagePanelDate).NotNull().GreaterThan(DateTimeOffset.MinValue).WithMessage("Tanggal Sample Jasa Sewing Tidak Boleh Kosong");
            // RuleFor(r => r.ServiceSampleSewingDate).NotNull().LessThan(DateTimeOffset.Now).WithMessage("Tanggal Sample Jasa Sewing Tidak Boleh Lebih dari Hari Ini");
            RuleFor(r => r.NettWeight).NotEmpty().WithMessage("Nett Weight tidak boleh kosong!").OverridePropertyName("Nett Weight");
            RuleFor(r => r.GrossWeight).NotEmpty().WithMessage("Gross Weight tidak boleh kosong!").OverridePropertyName("Gross Weight");
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleFor(r => r.Items).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount");
            RuleForEach(r => r.Items).SetValidator(new GarmentServiceSampleShrinkagePanelItemValueObjectValidator());
        }
    }

    class GarmentServiceSampleShrinkagePanelItemValueObjectValidator : AbstractValidator<GarmentServiceSampleShrinkagePanelItemValueObject>
    {
        public GarmentServiceSampleShrinkagePanelItemValueObjectValidator()
        {
            RuleFor(r => r.UnitSender).NotNull();
            RuleFor(r => r.UnitRequest).NotNull();
            RuleFor(r => r.Details).NotEmpty().OverridePropertyName("Details");
            RuleFor(r => r.Details).NotEmpty().WithMessage("Details Tidak Boleh Kosong").OverridePropertyName("DetailsCount");
            RuleFor(r => r.Details.Where(s => s.IsSave == true)).NotEmpty().WithMessage("Details Tidak Boleh Kosong").OverridePropertyName("DetailsCount").When(s => s.Details != null);
            RuleForEach(r => r.Details).SetValidator(new GarmentServiceSampleShrinkagePanelDetailValueObjectValidator());
        }
    }

    class GarmentServiceSampleShrinkagePanelDetailValueObjectValidator : AbstractValidator<GarmentServiceSampleShrinkagePanelDetailValueObject>
    {
        public GarmentServiceSampleShrinkagePanelDetailValueObjectValidator()
        {
            /*RuleFor(r => r.Quantity)
                .GreaterThan(0)
                .WithMessage("'Jumlah' harus lebih dari '0'.").When(r => r.IsSave == true);*/
        }
    }
}
