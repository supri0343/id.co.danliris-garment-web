using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSewingIns.ValueObjects;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingIns.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentSewingIns.Commands
{
    public class PlaceGarmentSubconSewingInCommand : ICommand<GarmentSubconSewingIn>
    {
        public string SewingInNo { get; set; }
        public string SewingFrom { get; set; }
        public Guid LoadingOutId { get; set; }
        public string LoadingOutNo { get; set; }
        public UnitDepartment UnitFrom { get; set; }
        public UnitDepartment Unit { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public GarmentComodity Comodity { get; set; }
        public DateTimeOffset? SewingInDate { get; set; }
        public DateTimeOffset? SewingDate { get; set; }
        public DateTimeOffset? FinishingDate { get; set; }
        public double Price { get; set; }

        public List<GarmentSubconSewingInItemValueObject> Items { get; set; }
    }

    public class PlaceGarmentSubconSewingInCommandValidator : AbstractValidator<PlaceGarmentSubconSewingInCommand>
    {
        public PlaceGarmentSubconSewingInCommandValidator()
        {
            RuleFor(r => r.UnitFrom).NotNull();
            RuleFor(r => r.UnitFrom.Id).NotEmpty().OverridePropertyName("UnitFrom").When(w => w.UnitFrom != null);

            RuleFor(r => r.Unit).NotNull();
            RuleFor(r => r.Unit.Id).NotEmpty().OverridePropertyName("Unit").When(w => w.Unit != null).WithMessage("Unit Sewing In Tidak Boleh Kosong");

            RuleFor(r => r.LoadingOutNo).NotNull().WithMessage("No Loading Tidak Boleh Kosong").When(r=>r.SewingFrom=="CUTTING");
            //RuleFor(r => r.LoadingId).NotEmpty();
            RuleFor(r => r.Article).NotNull();
            RuleFor(r => r.Comodity).NotNull();
            RuleFor(r => r.Comodity.Id).NotEmpty().OverridePropertyName("Comodity").When(w => w.Comodity != null);

            RuleFor(r => r.Price).GreaterThan(0).WithMessage("Tarif komoditi belum ada");

            RuleFor(r => r.RONo).NotNull();
            RuleFor(r => r.SewingInDate).NotNull().GreaterThan(DateTimeOffset.MinValue).WithMessage("Tanggal Sewing In Tidak Boleh Kosong");
            RuleFor(r => r.SewingInDate).NotNull().LessThan(DateTimeOffset.Now).WithMessage("Tanggal Sewing In Tidak Boleh Lebih dari Hari Ini");
            RuleFor(r => r.SewingInDate).NotNull().GreaterThan(r => r.SewingDate.GetValueOrDefault().Date).WithMessage(r => $"Tanggal Tidak Boleh Kurang dari tanggal {r.SewingDate.GetValueOrDefault().ToOffset(new TimeSpan(7, 0, 0)).ToString("dd/MM/yyyy", new CultureInfo("id-ID"))}").When(r => r.SewingFrom == "SEWING" && r.SewingDate != null);
            RuleFor(r => r.SewingInDate).NotNull().GreaterThan(r => r.FinishingDate.GetValueOrDefault().Date).WithMessage(r => $"Tanggal Tidak Boleh Kurang dari tanggal {r.FinishingDate.GetValueOrDefault().ToOffset(new TimeSpan(7, 0, 0)).ToString("dd/MM/yyyy", new CultureInfo("id-ID"))}").When(r => r.SewingFrom == "FINISHING" && r.FinishingDate != null);

            RuleFor(r => r.Items).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount");
            RuleFor(r => r.Items.Where(s => s.IsSave == true)).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount").When(s => s.Items != null);
            RuleForEach(r => r.Items).SetValidator(new GarmentSubconSewingInItemValueObjectValidator());
        }
    }

    class GarmentSubconSewingInItemValueObjectValidator : AbstractValidator<GarmentSubconSewingInItemValueObject>
    {
        public GarmentSubconSewingInItemValueObjectValidator()
        {
            RuleFor(r => r.Quantity)
                .GreaterThan(0)
                .WithMessage("Jumlah harus lebih besar dari 0")
                .When(w => w.IsSave == true);

            RuleFor(r => r.Quantity)
                .LessThanOrEqualTo(r => r.LoadingOutRemainingQuantity)
                .OverridePropertyName("Quantity")
                .WithMessage(x => $"'Jumlah' tidak boleh lebih dari '{x.LoadingOutRemainingQuantity}'.")
                .When(w => w.IsSave == true);
        }
    }
}