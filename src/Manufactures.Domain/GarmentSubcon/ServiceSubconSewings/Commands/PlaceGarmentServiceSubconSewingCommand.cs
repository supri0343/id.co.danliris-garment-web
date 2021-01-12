using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Commands
{
    public class PlaceGarmentServiceSubconSewingCommand : ICommand<GarmentServiceSubconSewing>
    {
        public string ServiceSubconSewingNo { get; set; }
        public Buyer Buyer { get; set; }
        public UnitDepartment Unit { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public GarmentComodity Comodity { get; set; }
        public DateTimeOffset? ServiceSubconSewingDate { get; set; }
        public bool IsDifferentSize { get; set; }
        public bool IsUsed { get; set; }
        public List<GarmentServiceSubconSewingItemValueObject> Items { get; set; }
        public bool IsSave { get; set; }


    }

    public class PlaceGarmentServiceSubconSewingCommandValidator : AbstractValidator<PlaceGarmentServiceSubconSewingCommand>
    {
        public PlaceGarmentServiceSubconSewingCommandValidator()
        {
            RuleFor(r => r.Unit).NotNull();
            RuleFor(r => r.Unit.Id).NotEmpty().OverridePropertyName("Unit").When(w => w.Unit != null);
            RuleFor(r => r.Article).NotNull();
            RuleFor(r => r.RONo).NotNull();
            RuleFor(r => r.ServiceSubconSewingDate).NotNull().GreaterThan(DateTimeOffset.MinValue).WithMessage("Tanggal Subcon Jasa Sewing Tidak Boleh Kosong");
            RuleFor(r => r.ServiceSubconSewingDate).NotNull().LessThan(DateTimeOffset.Now).WithMessage("Tanggal Subcon Jasa Sewing Tidak Boleh Lebih dari Hari Ini");
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleFor(r => r.Items).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount");
            RuleFor(r => r.Items.Where(s => s.IsSave == true)).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount").When(s => s.Items != null);
            RuleForEach(r => r.Items).SetValidator(new GarmentServiceSubconSewingItemValueObjectValidator());
        }
    }

    class GarmentServiceSubconSewingItemValueObjectValidator : AbstractValidator<GarmentServiceSubconSewingItemValueObject>
    {
        public GarmentServiceSubconSewingItemValueObjectValidator()
        {
            RuleFor(r => r.Quantity)
                .GreaterThan(0)
                .WithMessage("'Jumlah' harus lebih dari '0'.").When(r => r.IsSave == true);
            RuleFor(r => r.Quantity)
               .LessThanOrEqualTo(r => r.SewingInQuantity)
               .WithMessage(x => $"'Jumlah' tidak boleh lebih dari '{x.SewingInQuantity}'.").When(w => w.IsDifferentSize == false && w.IsSave == true);

            RuleFor(r => r.TotalQuantity)
               .LessThanOrEqualTo(r => r.SewingInQuantity)
               .WithMessage(x => $"'Jumlah Total Detail' tidak boleh lebih dari '{x.SewingInQuantity}'.").When(w => w.IsDifferentSize == true && w.IsSave == true);
        }
    }
}
