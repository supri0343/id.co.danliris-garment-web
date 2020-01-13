using FluentValidation;
using Infrastructure.Domain.Commands;

namespace Manufactures.Domain.GarmentScrapClassifications.Commands
{
	public class PlaceGarmentScrapClassificationCommand : ICommand<GarmentScrapClassification>
	{
		public string Code { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public class PlaceGarmentScrapClassificationCommandValidator : AbstractValidator<PlaceGarmentScrapClassificationCommand>
		{
			public PlaceGarmentScrapClassificationCommandValidator()
			{
				RuleFor(r => r.Code).NotNull().WithMessage("Kode Jenis Barang Aval sudah ada");
				RuleFor(r => r.Name).NotNull().WithMessage("Nama Jenis Barang Aval sudah ada");
				
			}
		}
	}
}
