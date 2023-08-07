using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentLoadings.ValueObjects;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingIns.ValueObjects;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingOuts.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentLoadingOuts.Commands
{
    public class PlaceGarmentSubconLoadingOutCommand : ICommand<GarmentSubconLoadingOut>
    {
        public string LoadingOutNo { get; set; }
        public Guid LoadingInId { get;  set; }
        public string LoadingInNo { get;  set; }
        public UnitDepartment UnitFrom { get; set; }
        public UnitDepartment Unit { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public GarmentComodity Comodity { get; set; }
        public DateTimeOffset LoadingOutDate { get; set; }
        public DateTimeOffset? LoadingInDate { get; set; }
        public double Price { get; set; }

        public List<GarmentSubconLoadingOutItemValueObject> Items { get; set; }
    }

    public class PlaceGarmentLoadingCommandValidator : AbstractValidator<PlaceGarmentSubconLoadingOutCommand>
    {
        public PlaceGarmentLoadingCommandValidator()
        {
            RuleFor(r => r.Unit).NotNull();
            RuleFor(r => r.Unit.Id).NotEmpty().OverridePropertyName("Unit").When(w => w.Unit != null);
            RuleFor(r => r.LoadingInNo).NotNull().WithMessage("Nomor Loading In Tidak Boleh Kosong");
            RuleFor(r => r.RONo).NotNull();
            RuleFor(r => r.LoadingOutDate).NotNull().GreaterThan(DateTimeOffset.MinValue).WithMessage("Tanggal Loading Tidak Boleh Kosong");
            RuleFor(r => r.LoadingOutDate).NotNull().LessThan(DateTimeOffset.Now).WithMessage("Tanggal Loading Tidak Boleh Lebih dari Hari Ini");
            RuleFor(r => r.LoadingOutDate).NotNull().GreaterThan(r => r.LoadingInDate.GetValueOrDefault().Date).WithMessage(r => $"Tanggal Loading Tidak Boleh Kurang dari tanggal {r.LoadingInDate.GetValueOrDefault().ToOffset(new TimeSpan(7, 0, 0)).ToString("dd/MM/yyyy", new CultureInfo("id-ID"))}").When(r=>r.LoadingInDate != null);
            RuleFor(r => r.Comodity).NotNull();

            RuleFor(r => r.Price).GreaterThan(0).WithMessage("Tarif komoditi belum ada");
            RuleFor(r => r.Article).NotNull();
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleFor(r => r.Items).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount");
            RuleFor(r => r.Items.Where(s => s.IsSave == true)).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount").When(s => s.Items != null);
            RuleForEach(r => r.Items).SetValidator(new GarmentSubconLoadingOutItemValueObjectValidator());
        }
    }

    class GarmentSubconLoadingOutItemValueObjectValidator : AbstractValidator<GarmentSubconLoadingOutItemValueObject>
    {
        public GarmentSubconLoadingOutItemValueObjectValidator()
        {
            RuleFor(r => r.Quantity)
                .GreaterThan(0)
                .WithMessage("'Jumlah' harus lebih dari '0'.")
                .When(w => w.IsSave);

            RuleFor(r => r.Quantity)
                .LessThanOrEqualTo(r => r.LoadingInRemainingQuantity)
                .OverridePropertyName("Quantity")
                .WithMessage(x => $"'Jumlah' tidak boleh lebih dari '{x.LoadingInRemainingQuantity}'.")
                .When(w => w.IsSave == true);
        }
    }
}
