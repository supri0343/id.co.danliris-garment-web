using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentPreparings.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentPreparings.Commands
{
    public class PlaceGarmentPreparingCommand : ICommand<GarmentPreparing>
    {
        public int UENId { get; set; }
        public string UENNo { get; set; }
        public UnitDepartment Unit { get; set; }
        public DateTimeOffset? ProcessDate { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public bool IsCuttingIn { get; set; }
        public List<GarmentPreparingItemValueObject> Items { get; set; }
    }

    public class PlaceGarmentPreparingCommandValidator : AbstractValidator<PlaceGarmentPreparingCommand>
    {
        public PlaceGarmentPreparingCommandValidator()
        {
            RuleFor(r => r.UENId).NotEmpty().WithMessage("Nomor Bon Pengeluaran Unit Tidak Boleh Kosong");
            RuleFor(r => r.ProcessDate).NotNull().WithMessage("Tanggal Proses Tidak Boleh Kosong");
            RuleFor(r => r.Items).NotEmpty().WithMessage("Item Tidak Boleh Kosong");
        }
    }    
}