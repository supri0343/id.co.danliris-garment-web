using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.CustomsOuts.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.CustomsOuts.Commands
{
    public class PlaceGarmentSubconCustomsOutCommand : ICommand<GarmentSubconCustomsOut>
    {
        public string CustomsOutNo { get;  set; }
        public DateTimeOffset CustomsOutDateDate { get;  set; }
        public string CustomsOutType { get;  set; }
        public string SubconType { get;  set; }
        public Guid SubconContractId { get;  set; }
        public string SubconContractNo { get;  set; }
        public Supplier Supplier { get; set; }
        public string Remark { get;  set; }
        public double TotalQty { get; set; }
        public double UsedQty { get; set; }
        public virtual List<GarmentSubconCustomsOutItemValueObject> Items { get;  set; }
    }

    public class PlaceGarmentSubconCustomsOutCommandValidator : AbstractValidator<PlaceGarmentSubconCustomsOutCommand>
    {
        public PlaceGarmentSubconCustomsOutCommandValidator()
        {
            RuleFor(r => r.SubconContractId).NotNull();
            RuleFor(r => r.SubconContractNo).NotNull();
            RuleFor(r => r.CustomsOutDateDate).NotNull().GreaterThan(DateTimeOffset.MinValue);
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleFor(r => r.Items).NotEmpty().WithMessage("Item tidak boleh kosong").OverridePropertyName("ItemsCount").When(s => s.Items != null);
            RuleForEach(r => r.Items).SetValidator(new GarmentSubconCustomsOutItemValueObjectValidator()).When(s => s.Items != null);
            RuleFor(r => r.TotalQty)
                 .LessThanOrEqualTo(r => r.UsedQty)
                 .WithMessage(x => $"'Jumlah Total' tidak boleh lebih dari '{x.UsedQty}'.");
        }
    }

    public class GarmentSubconCustomsOutItemValueObjectValidator : AbstractValidator<GarmentSubconCustomsOutItemValueObject>
    {
        public GarmentSubconCustomsOutItemValueObjectValidator()
        {

            RuleFor(r => r.Quantity)
                .GreaterThan(0)
                .WithMessage("'Jumlah' harus lebih dari '0'.");
        }
    }
}
