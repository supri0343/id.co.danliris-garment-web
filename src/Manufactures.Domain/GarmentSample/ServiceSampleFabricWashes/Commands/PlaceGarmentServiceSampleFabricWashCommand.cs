using Infrastructure.Domain.Commands;
using FluentValidation;
using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes.Commands
{
    public class PlaceGarmentServiceSampleFabricWashCommand : ICommand<GarmentServiceSampleFabricWash>
    {
        public string ServiceSampleFabricWashNo { get; set; }
        public DateTimeOffset? ServiceSampleFabricWashDate { get; set; }
        public string Remark { get; set; }
        public bool IsUsed { get; set; }
        public int QtyPacking { get; set; }
        public string UomUnit { get; set; }
        public double NettWeight { get; set; }
        public double GrossWeight { get; set; }
        public List<GarmentServiceSampleFabricWashItemValueObject> Items { get; set; }
        public bool IsSave { get; set; }
    }
    public class PlaceGarmentServiceSampleFabricWashCommandValidator : AbstractValidator<PlaceGarmentServiceSampleFabricWashCommand>
    {
        public PlaceGarmentServiceSampleFabricWashCommandValidator()
        {
            RuleFor(r => r.ServiceSampleFabricWashDate).NotNull().GreaterThan(DateTimeOffset.MinValue).WithMessage("Tanggal Sample Jasa Fabric Wash Tidak Boleh Kosong");
            // RuleFor(r => r.ServiceSampleSewingDate).NotNull().LessThan(DateTimeOffset.Now).WithMessage("Tanggal Sample Jasa Sewing Tidak Boleh Lebih dari Hari Ini");
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleFor(r => r.Items).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount");
            RuleForEach(r => r.Items).SetValidator(new GarmentServiceSampleFabricWashItemValueObjectValidator());
        }
    }

    class GarmentServiceSampleFabricWashItemValueObjectValidator : AbstractValidator<GarmentServiceSampleFabricWashItemValueObject>
    {
        public GarmentServiceSampleFabricWashItemValueObjectValidator()
        {
            RuleFor(r => r.UnitSender).NotNull();
            RuleFor(r => r.UnitRequest).NotNull();
            RuleFor(r => r.Details).NotEmpty().OverridePropertyName("Details");
            RuleFor(r => r.Details).NotEmpty().WithMessage("Details Tidak Boleh Kosong").OverridePropertyName("DetailsCount");
            //RuleFor(r => r.Details.Where(s => s.IsSave == true)).NotEmpty().WithMessage("Details Tidak Boleh Kosong").OverridePropertyName("DetailsCount").When(s => s.Details != null);
            RuleForEach(r => r.Details).SetValidator(new GarmentServiceSampleFabricWashDetailValueObjectValidator());
        }
    }

    class GarmentServiceSampleFabricWashDetailValueObjectValidator : AbstractValidator<GarmentServiceSampleFabricWashDetailValueObject>
    {
        public GarmentServiceSampleFabricWashDetailValueObjectValidator()
        {
            /*RuleFor(r => r.Quantity)
                .GreaterThan(0)
                .WithMessage("'Jumlah' harus lebih dari '0'.").When(r => r.IsSave == true);*/
        }
    }
}
