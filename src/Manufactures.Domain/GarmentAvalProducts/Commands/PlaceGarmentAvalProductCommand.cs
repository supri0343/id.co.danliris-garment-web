using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentAvalProducts.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;


namespace Manufactures.Domain.GarmentAvalProducts.Commands
{
    public class PlaceGarmentAvalProductCommand : ICommand<GarmentAvalProduct>
    {
        public string RONo { get; set; }
        public string Article { get; set; }
        public DateTimeOffset AvalDate { get; set; }
        public List<GarmentAvalProductItemValueObject> Items { get; set; }
    }
    public class PlaceGarmentAvalProductCommandValidator : AbstractValidator<PlaceGarmentAvalProductCommand>
    {
        public PlaceGarmentAvalProductCommandValidator()
        {
            RuleFor(r => r.RONo).NotNull().WithMessage("Nomor RO Tidak Boleh Kosong");
            RuleFor(r => r.Items).NotEmpty().WithMessage("Item Tidak Boleh Kosong");
        }
    }
}