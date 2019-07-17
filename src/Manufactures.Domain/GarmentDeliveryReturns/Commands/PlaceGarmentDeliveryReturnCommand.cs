﻿using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentDeliveryReturns.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentDeliveryReturns.Commands
{
    public class PlaceGarmentDeliveryReturnCommand : ICommand<GarmentDeliveryReturn>
    {
        public string DRNo { get; set; } 
        public string RONo { get; set; }
        public string Article { get; set; }
        public int UnitDOId { get; set; }
        public string UnitDONo { get; set; }
        public int UENId { get; set; }
        public string PreparingId { get; set; }
        public DateTimeOffset ReturnDate { get; set; }
        public string ReturnType { get; set; }
        public UnitDepartment Unit { get; set; }
        public Storage Storage { get; set; }
        public bool IsUsed { get; set; }
        public List<GarmentDeliveryReturnItemValueObject> Items { get; set; }
    }

    public class PlaceGarmentDeliveryReturnCommandValidator : AbstractValidator<PlaceGarmentDeliveryReturnCommand>
    {
        public PlaceGarmentDeliveryReturnCommandValidator()
        {
            RuleFor(r => r.RONo).NotNull().WithMessage("Nomor RO Tidak Boleh Kosong");
            RuleFor(r => r.Items).NotEmpty().WithMessage("Item Tidak Boleh Kosong");
            RuleForEach(r => r.Items).SetValidator(new PlaceGarmentDeliveryReturnItemValueObjectValidator());
        }
    }
    class PlaceGarmentDeliveryReturnItemValueObjectValidator : AbstractValidator<GarmentDeliveryReturnItemValueObject>
    {
        public PlaceGarmentDeliveryReturnItemValueObjectValidator()
        {
            RuleFor(r => r.Quantity).GreaterThan(r => r.QuantityUENItem).WithMessage("Quantity tidak boleh Lebih Dari Quantity pada Bon Pengeluaran Unit").When(w => w.Product.Name != "FABRIC");
            RuleFor(r => r.Quantity).GreaterThan(r => r.RemainingQuantityPreparingItem).WithMessage("Quantity tidak boleh Lebih Dari RemainingQuantity pada Preparing").When(w => w.Product.Name == "FABRIC");
        }
    }
    
}