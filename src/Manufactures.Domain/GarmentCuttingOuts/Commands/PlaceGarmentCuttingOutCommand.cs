﻿using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentCuttingOuts.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentCuttingOuts.Commands
{
    public class PlaceGarmentCuttingOutCommand : ICommand<GarmentCuttingOut>
    {
        public string CutOutNo { get; set; }
        public string CuttingOutType { get; set; }

        public UnitDepartment UnitFrom { get; set; }
        public DateTimeOffset? CuttingOutDate { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public UnitDepartment Unit { get; set; }
        public GarmentComodity Comodity { get; set; }
        public List<GarmentCuttingOutItemValueObject> Items { get; set; }
    }

    public class PlaceGarmentCuttingOutCommandValidator : AbstractValidator<PlaceGarmentCuttingOutCommand>
    {
        public PlaceGarmentCuttingOutCommandValidator()
        {
            RuleFor(r => r.UnitFrom).NotNull();
            RuleFor(r => r.UnitFrom.Id).NotEmpty().OverridePropertyName("UnitFrom").When(w => w.Unit != null);

            RuleFor(r => r.Unit).NotNull();
            RuleFor(r => r.Unit.Id).NotEmpty().OverridePropertyName("Unit").When(w => w.Unit != null);

            RuleFor(r => r.RONo).NotNull();
            RuleFor(r => r.CuttingOutDate).NotNull().GreaterThan(DateTimeOffset.MinValue);
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleFor(r => r.Items).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount");
            RuleFor(r => r.Items.Where(s => s.IsSave == true)).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount").When(s=>s.Items != null);
            RuleForEach(r => r.Items).SetValidator(new GarmentCuttingOutItemValueObjectValidator());
        }
    }

    class GarmentCuttingOutItemValueObjectValidator : AbstractValidator<GarmentCuttingOutItemValueObject>
    {
        public GarmentCuttingOutItemValueObjectValidator()
        {
            RuleFor(r => r.Details).NotEmpty().OverridePropertyName("Detail").When(w => w.IsSave == true);

            RuleFor(r => r.TotalCuttingOutQuantity)
               .LessThanOrEqualTo(r => r.TotalCuttingOut)
               .WithMessage(x => $"'Jumlah Potong' tidak boleh lebih dari '{x.TotalCuttingOut}'.");

            RuleForEach(r => r.Details).SetValidator(new GarmentCuttingOutDetailValueObjectValidator()).When(w => w.IsSave == true);
        }
    }

    class GarmentCuttingOutDetailValueObjectValidator : AbstractValidator<GarmentCuttingOutDetailValueObject>
    {
        public GarmentCuttingOutDetailValueObjectValidator()
        {
            // not supported at this time: FluentValidation 8.0.100 but some libs are incompatible
            //When(w => w.IsSave, () =>
            //{
            //    //RuleFor(r => r.PreparingUom).NotNull();
            //    //RuleFor(r => r.PreparingUom.Unit).NotEmpty().OverridePropertyName("PreparingUom").When(w => w.PreparingUom != null);
            //    ////RuleFor(r => r.PreparingUom).Must(m => !string.IsNullOrWhiteSpace(m.Unit)).When(w => w.PreparingUom != null);

            //    RuleFor(r => r.CuttingInQuantity).GreaterThan(0);

            //    RuleFor(r => r.PreparingQuantity).LessThanOrEqualTo(r => r.PreparingRemainingQuantity);
            //});

            RuleFor(r => r.Size).NotNull();
            RuleFor(r => r.Size.Id).NotEmpty().OverridePropertyName("Size").When(w => w.Size != null);

            RuleFor(r => r.CuttingOutQuantity)
                .GreaterThan(0)
                .WithMessage("'Jumlah Potong' harus lebih dari '0'.");

           
        }
    }
}