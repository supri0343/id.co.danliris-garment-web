using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentScrapTransactions.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentScrapTransactions.Commands
{
	public class PlaceGarmentScrapTransactionCommand : ICommand<GarmentScrapTransaction>
	{
		public string TransactionNo { get;  set; }
		public DateTimeOffset TransactionDate { get;  set; }
		public string TransactionType { get;  set; }
		public Guid ScrapSourceId { get;  set; }
		public string ScrapSourceName { get;  set; }
		public Guid ScrapDestinationId { get;  set; }
		public string ScrapDestinationName { get;  set; }
		public List<GarmentScrapTransactionItemValueObject> Items { get; set; }

	}

	public class PlaceGarmentScrapTransactionCommandValidator : AbstractValidator<PlaceGarmentScrapTransactionCommand>
	{
		public PlaceGarmentScrapTransactionCommandValidator()
		{
			
			RuleFor(r => r.TransactionDate).NotNull().GreaterThan(DateTimeOffset.MinValue).WithMessage("Tanggal harus diisi");
			RuleFor(r => r.ScrapSourceName).NotEmpty().WithMessage("Asal Barang harus diisi");
			RuleFor(r => r.ScrapDestinationName).NotEmpty().WithMessage("Tujuan Barang harus diisi");
			RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
			RuleFor(r => r.Items).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount");
			RuleFor(r => r.Items).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount").When(s => s.Items != null);
			RuleForEach(r => r.Items).SetValidator(new GarmentScrapTransactionItemValueObjectValidator());
		}
	}

	class GarmentScrapTransactionItemValueObjectValidator : AbstractValidator<GarmentScrapTransactionItemValueObject>
	{
		public GarmentScrapTransactionItemValueObjectValidator()
		{
			//RuleFor(r => r.Quantity)
			//	.GreaterThan(0)
			//	.WithMessage("'Jumlah' harus lebih dari '0'.");
			
		}
	}
}
