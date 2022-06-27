using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.SampleReceiptFromBuyers.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleReceiptFromBuyers.Commands
{
	public class PlaceGarmentSampleReceiptFromBuyerCommand : ICommand<GarmentSampleReceiptFromBuyer>
	{
		public string SaveAs { get;   set; }
		public DateTimeOffset ReceiptDate { get;   set; }
		public List<GarmentSampleFromBuyerItemValueObject> Items { get; set; }
	}

	public class PlaceGarmentSampleReceiptFromBuyerCommandValidator : AbstractValidator<PlaceGarmentSampleReceiptFromBuyerCommand>
	{
		public PlaceGarmentSampleReceiptFromBuyerCommandValidator()
		{
			RuleFor(r => r.ReceiptDate).NotNull();
			 
			RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
			RuleFor(r => r.Items).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount"); 
			RuleForEach(r => r.Items).SetValidator(new GarmentSampleReceiptFromBuyerItemValueObjectValidator());
		}
	}

	class GarmentSampleReceiptFromBuyerItemValueObjectValidator : AbstractValidator<GarmentSampleFromBuyerItemValueObject>
	{
		public GarmentSampleReceiptFromBuyerItemValueObjectValidator()
		{
			RuleFor(r => r.ReceiptQuantity)
				.GreaterThan(0)
				.WithMessage("'Jumlah' harus lebih dari '0'.");

			RuleFor(r => r.SizeName)
				.NotNull()
				.WithMessage("'Size' harus diisi'.");

			RuleFor(r => r.RONo)
			.NotNull()
			.WithMessage("'RONo' harus diisi'.");

			RuleFor(r => r.Style)
			.NotNull()
			.WithMessage("'Style' harus diisi'.");



		}
	}
}
