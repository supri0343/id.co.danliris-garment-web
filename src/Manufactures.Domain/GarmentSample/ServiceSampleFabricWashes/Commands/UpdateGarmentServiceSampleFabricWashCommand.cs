using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes.Commands
{
    public class UpdateGarmentServiceSampleFabricWashCommand : ICommand<GarmentServiceSampleFabricWash>
    {
        public Guid Identity { get; private set; }
        public string ServiceSubconFabricWashNo { get; set; }
        public DateTimeOffset? ServiceSubconFabricWashDate { get; set; }
        public string Remark { get; set; }
        public bool IsUsed { get; set; }
        public int QtyPacking { get; set; }
        public string UomUnit { get; set; }
        public double NettWeight { get; set; }
        public double GrossWeight { get; set; }
        public List<GarmentServiceSampleFabricWashItemValueObject> Items { get; set; }

        public void SetIdentity(Guid id)
        {
            Identity = id;
        }
    }

    public class UpdateGarmentServiceSampleFabricWashCommandValidator : AbstractValidator<UpdateGarmentServiceSampleFabricWashCommand>
    {
        public UpdateGarmentServiceSampleFabricWashCommandValidator()
        {
            RuleFor(r => r.ServiceSubconFabricWashDate).NotNull().GreaterThan(DateTimeOffset.MinValue).WithMessage("Tanggal Sample Jasa Fabric Wash Tidak Boleh Kosong");
            //RuleFor(r => r.ServiceSampleSewingDate).NotNull().LessThan(DateTimeOffset.Now).WithMessage("Tanggal Sample Jasa Sewing Tidak Boleh Lebih dari Hari Ini");
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleFor(r => r.Items).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount");
            RuleForEach(r => r.Items).SetValidator(new GarmentServiceSampleFabricWashItemValueObjectValidator());
        }
    }
}
