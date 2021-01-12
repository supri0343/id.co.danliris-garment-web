using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Commands
{
    public class UpdateGarmentServiceSubconSewingCommand : ICommand<GarmentServiceSubconSewing>
    {
        public Guid Identity { get; private set; }
        public string ServiceSubconSewingNo { get; set; }
        public Buyer Buyer { get; set; }
        public UnitDepartment Unit { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public GarmentComodity Comodity { get; set; }
        public DateTimeOffset? ServiceSubconSewingDate { get; set; }
        public bool IsDifferentSize { get; set; }
        public List<GarmentServiceSubconSewingItemValueObject> Items { get; set; }

        public void SetIdentity(Guid id)
        {
            Identity = id;
        }
    }

    public class UpdateGarmentServiceSubconSewingCommandValidator : AbstractValidator<UpdateGarmentServiceSubconSewingCommand>
    {
        public UpdateGarmentServiceSubconSewingCommandValidator()
        {
            RuleFor(r => r.Unit).NotNull();
            RuleFor(r => r.Unit.Id).NotEmpty().OverridePropertyName("Unit").When(w => w.Unit != null);

            RuleFor(r => r.Article).NotNull();
            RuleFor(r => r.RONo).NotNull();
            RuleFor(r => r.ServiceSubconSewingDate).NotNull().GreaterThan(DateTimeOffset.MinValue);
            RuleFor(r => r.ServiceSubconSewingDate).NotNull().LessThan(DateTimeOffset.Now).WithMessage("Tanggal Service Subcon Sewing Tidak Boleh Lebih dari Hari Ini");
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleFor(r => r.Items.Where(s => s.IsSave == true)).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount").When(s => s.Items != null);
            RuleForEach(r => r.Items).SetValidator(new GarmentServiceSubconSewingItemValueObjectValidator());
        }
    }
}
