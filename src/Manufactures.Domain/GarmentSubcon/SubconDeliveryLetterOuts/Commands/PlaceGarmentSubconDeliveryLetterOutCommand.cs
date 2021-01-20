using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Commands
{
    public class PlaceGarmentSubconDeliveryLetterOutCommand : ICommand<GarmentSubconDeliveryLetterOut>
    {
        public string DLNo { get; set; }
        public string DLType { get; set; }
        public Guid SubconContractId { get; set; }
        public string ContractNo { get; set; }
        public string ContractType { get; set; }
        public DateTimeOffset DLDate { get; set; }

        public int UENId { get; set; }
        public string UENNo { get; set; }

        public string PONo { get; set; }
        public int EPOItemId { get; set; }

        public string Remark { get; set; }
        public bool IsUsed { get; set; }
        public double TotalQty { get; set; }
        public double UsedQty { get; set; }
        public List<GarmentSubconDeliveryLetterOutItemValueObject> Items { get; set; }
    }

    public class PlaceGarmentSubconDeliveryLetterOutCommandValidator : AbstractValidator<PlaceGarmentSubconDeliveryLetterOutCommand>
    {
        public PlaceGarmentSubconDeliveryLetterOutCommandValidator()
        {
            RuleFor(r => r.SubconContractId).NotNull();
            RuleFor(r => r.ContractNo).NotNull();
            RuleFor(r => r.UENId).NotEmpty();
            RuleFor(r => r.DLDate).NotNull().GreaterThan(DateTimeOffset.MinValue);
            RuleFor(r => r.UENNo).NotNull();
            RuleFor(r => r.PONo).NotNull();
            RuleFor(r => r.EPOItemId).NotNull();
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleFor(r => r.Items).NotEmpty().WithMessage("Item tidak boleh kosong").OverridePropertyName("ItemsCount").When(s => s.Items != null);
            RuleForEach(r => r.Items).SetValidator(new GarmentSubconDeliveryLetterOutItemValueObjectValidator());
            RuleFor(r => r.TotalQty)
                 .LessThanOrEqualTo(r => r.UsedQty)
                 .WithMessage(x => $"'Jumlah Total' tidak boleh lebih dari '{x.UsedQty}'.");
        }
    }

    class GarmentSubconDeliveryLetterOutItemValueObjectValidator : AbstractValidator<GarmentSubconDeliveryLetterOutItemValueObject>
    {
        public GarmentSubconDeliveryLetterOutItemValueObjectValidator()
        {

            RuleFor(r => r.Quantity)
                .GreaterThan(0)
                .WithMessage("'Jumlah' harus lebih dari '0'.");

            RuleFor(r => r.Quantity)
                .LessThanOrEqualTo(r => r.ContractQuantity)
                .WithMessage(x => $"'Jumlah' tidak boleh lebih dari '{x.ContractQuantity}'.");
        }
    }
}
