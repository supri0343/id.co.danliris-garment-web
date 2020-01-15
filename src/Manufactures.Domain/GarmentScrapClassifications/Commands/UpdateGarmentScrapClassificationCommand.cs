using FluentValidation;
using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentScrapClassifications.Commands
{
	public class UpdateGarmentScrapClassificationCommand : ICommand<GarmentScrapClassification>
	{
		public Guid Identity { get; private set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
	
		public void SetIdentity(Guid id)
		{
			Identity = id;
		}
		public class UpdateGarmentScrapClassificationCommandValidator : AbstractValidator<UpdateGarmentScrapClassificationCommand>
		{
			public UpdateGarmentScrapClassificationCommandValidator()
			{
				RuleFor(r => r.Code).NotNull().WithMessage("Kode Jenis Barang Aval sudah ada");
				RuleFor(r => r.Name).NotNull().WithMessage("Nama Jenis Barang Aval sudah ada");

			}
		}
	}
}
