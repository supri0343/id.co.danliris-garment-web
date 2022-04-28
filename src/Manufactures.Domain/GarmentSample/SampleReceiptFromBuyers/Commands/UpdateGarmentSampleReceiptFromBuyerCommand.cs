using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.SampleReceiptFromBuyers.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleReceiptFromBuyers.Commands
{
	public class UpdateGarmentSampleReceiptFromBuyerCommand : ICommand<GarmentSampleReceiptFromBuyer>
	{
		public Guid Identity { get; private set; }
		public string SaveAs { get;  set; }
		public DateTimeOffset ReceiptDate { get;  set; }
		public List<GarmentSampleFromBuyerItemValueObject> Items { get; set; }
		public void SetIdentity(Guid id)
		{
			Identity = id;
		}
		public class UpdateGarmentSampleReceiptFromBuyerCommandValidator : AbstractValidator<UpdateGarmentSampleReceiptFromBuyerCommand>
		{
			public UpdateGarmentSampleReceiptFromBuyerCommandValidator()
			{
				//RuleFor(r => r.SaveAs).NotNull();
				//RuleFor(r => r.ReceiptDate).NotNull().GreaterThan(DateTimeOffset.MinValue).WithMessage("Tanggal Tidak Boleh Kosong");
				 
			}
		}
	}
}
