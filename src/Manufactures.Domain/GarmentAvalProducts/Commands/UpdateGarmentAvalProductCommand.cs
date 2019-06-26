using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentAvalProducts.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;


namespace Manufactures.Domain.GarmentAvalProducts.Commands
{
    public class UpdateGarmentAvalProductCommand : ICommand<GarmentAvalProduct>
    {
        public void SetId(Guid id) { Id = id; }
        public Guid Id { get; private set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public DateTimeOffset? AvalDate { get; set; }
        public List<GarmentAvalProductItemValueObject> Items { get; set; }
    }

    public class UpdateGarmentAvalProductCommandValidator : AbstractValidator<UpdateGarmentAvalProductCommand>
    {
        public UpdateGarmentAvalProductCommandValidator()
        {
            RuleFor(r => r.RONo).NotEmpty().WithMessage("Nomor RO Tidak Boleh Kosong");
            RuleFor(r => r.AvalDate).NotNull().WithMessage("Tanggal Aval Tidak Boleh Kosong");
            RuleFor(r => r.Items).NotEmpty().WithMessage("Item Tidak Boleh Kosong");
        }
    }
}