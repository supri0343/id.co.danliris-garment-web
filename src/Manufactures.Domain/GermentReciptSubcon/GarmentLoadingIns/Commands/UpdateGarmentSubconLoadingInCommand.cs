using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentLoadings.ValueObjects;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingIns.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentLoadingIns.Commands
{
    public class UpdateGarmentSubconLoadingInCommand : ICommand<GarmentSubconLoadingIn>
    {
        public Guid Identity { get; private set; }
        public string LoadingNo { get;  set; }
        public Guid CuttingOutId { get; set; }
        public string CuttingOutNo { get; set; }
        public UnitDepartment UnitFrom { get;  set; }
        public UnitDepartment Unit { get;  set; }
        public string RONo { get;  set; }
        public string Article { get;  set; }
        public GarmentComodity Comodity { get;  set; }
        public DateTimeOffset LoadingDate { get;  set; }
        public DateTimeOffset? CuttingOutDate { get; set; }

        public List<GarmentSubconLoadingInItemValueObject> Items { get;  set; }

        public void SetIdentity(Guid id)
        {
            Identity = id;
        }
    }

    public class UpdateGarmentLoadingCommandValidator : AbstractValidator<UpdateGarmentSubconLoadingInCommand>
    {
        public UpdateGarmentLoadingCommandValidator()
        {
            RuleFor(r => r.Unit).NotNull();
            RuleFor(r => r.Unit.Id).NotEmpty().OverridePropertyName("Unit").When(w => w.Unit != null);
            RuleFor(r => r.CuttingOutId).NotNull();
            RuleFor(r => r.RONo).NotNull();
            RuleFor(r => r.Article).NotNull();
            RuleFor(r => r.LoadingDate).NotNull().GreaterThan(DateTimeOffset.MinValue);
            RuleFor(r => r.LoadingDate).NotNull().LessThan(DateTimeOffset.Now).WithMessage("Tanggal Loading Tidak Boleh Lebih dari Hari Ini");
            RuleFor(r => r.LoadingDate).NotNull().GreaterThan(r => r.CuttingOutDate.GetValueOrDefault().Date).WithMessage(r => $"Tanggal Loading Tidak Boleh Kurang dari tanggal {r.CuttingOutDate.GetValueOrDefault().ToOffset(new TimeSpan(7, 0, 0)).ToString("dd/MM/yyyy", new CultureInfo("id-ID"))}").When(r=>r.CuttingOutDate!=null);
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleForEach(r => r.Items).SetValidator(new GarmentSubconLoadingInItemValueObjectValidator());
        }
    }
}
