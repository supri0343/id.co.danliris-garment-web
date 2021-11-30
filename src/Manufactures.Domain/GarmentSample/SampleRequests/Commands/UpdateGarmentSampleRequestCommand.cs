using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.SampleRequests.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleRequests.Commands
{
    public class UpdateGarmentSampleRequestCommand : ICommand<GarmentSampleRequest>
    {
        public Guid Identity { get; private set; }
        public string SampleCategory { get; set; }
        public string SampleRequestNo { get; set; }
        public string RONoSample { get; set; }
        public string RONoCC { get; set; }
        public DateTimeOffset Date { get; set; }

        public Buyer Buyer { get; set; }

        public GarmentComodity Comodity { get; set; }

        public string SampleType { get; set; }
        public string Packing { get; set; }
        public DateTimeOffset SentDate { get; set; }
        public string POBuyer { get; set; }
        public string Attached { get; set; }
        public string Remark { get; set; }
        public bool IsPosted { get; set; }
        public bool IsReceived { get; set; }
        public List<GarmentSampleRequestProductValueObject> SampleProducts { get; set; }
        public List<GarmentSampleRequestSpecificationValueObject> SampleSpecifications { get; set; }
        public void SetIdentity(Guid id)
        {
            Identity = id;
        }
    }
    public class UpdateGarmentSampleRequestCommandValidator : AbstractValidator<UpdateGarmentSampleRequestCommand>
    {
        public UpdateGarmentSampleRequestCommandValidator()
        {
            RuleFor(r => r.Comodity).NotNull();
            RuleFor(r => r.Comodity.Id).NotEmpty().OverridePropertyName("Comodity").When(w => w.Comodity != null);

            RuleFor(r => r.Buyer).NotNull();
            RuleFor(r => r.Buyer.Id).NotEmpty().OverridePropertyName("Buyer").When(w => w.Buyer != null);

            RuleFor(r => r.SampleType).NotNull();
            RuleFor(r => r.RONoCC).NotNull();
            RuleFor(r => r.Packing).NotNull();
            RuleFor(r => r.Date).NotNull().GreaterThan(DateTimeOffset.MinValue).WithMessage("Tanggal Pembuatan Surat Sample Tidak Boleh Kosong");
            RuleFor(r => r.SentDate).NotNull().GreaterThan(DateTimeOffset.MinValue).WithMessage("Tanggal Kirim Tidak Boleh Kosong");
            RuleFor(r => r.SampleProducts).NotEmpty().OverridePropertyName("SampleProducts");
            RuleFor(r => r.SampleProducts).NotEmpty().WithMessage("Detail Barang tidak boleh kosong").OverridePropertyName("SampleProductsCount").When(s => s.SampleProducts != null);
            RuleForEach(r => r.SampleProducts).SetValidator(new GarmentSampleRequestProductValueObjectValidator());
            RuleFor(r => r.SampleSpecifications).NotEmpty().OverridePropertyName("SampleSpecifications");
            RuleFor(r => r.SampleSpecifications).NotEmpty().WithMessage("Kelengkapan Sample tidak boleh kosong").OverridePropertyName("ItemsCount").When(s => s.SampleSpecifications != null);
            RuleForEach(r => r.SampleSpecifications).SetValidator(new GarmentSampleRequestSpecificationValueObjectValidator());
        }
    }
}
