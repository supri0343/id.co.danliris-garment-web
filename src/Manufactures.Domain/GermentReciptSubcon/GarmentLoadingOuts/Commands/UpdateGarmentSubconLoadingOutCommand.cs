using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentLoadings.ValueObjects;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingIns.ValueObjects;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingOuts.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentLoadingOuts.Commands
{
    public class UpdateGarmentSubconLoadingOutCommand : ICommand<GarmentSubconLoadingOut>
    {
        public Guid Identity { get; private set; }
        public string LoadingOutNo { get; set; }
        public Guid LoadingInId { get; internal set; }
        public string LoadingInNo { get; internal set; }
        public UnitDepartment UnitFrom { get;  set; }
        public UnitDepartment Unit { get;  set; }
        public string RONo { get;  set; }
        public string Article { get;  set; }
        public GarmentComodity Comodity { get;  set; }
        public DateTimeOffset LoadingOutDate { get;  set; }
        public DateTimeOffset? LoadingInDate { get; set; }

        public List<GarmentSubconLoadingOutItemValueObject> Items { get;  set; }

        public void SetIdentity(Guid id)
        {
            Identity = id;
        }
    }

    public class UpdateGarmentLoadingCommandValidator : AbstractValidator<UpdateGarmentSubconLoadingOutCommand>
    {
        public UpdateGarmentLoadingCommandValidator()
        {
            RuleFor(r => r.Unit).NotNull();
            RuleFor(r => r.Unit.Id).NotEmpty().OverridePropertyName("Unit").When(w => w.Unit != null);
            RuleFor(r => r.LoadingInId).NotNull();
            RuleFor(r => r.RONo).NotNull();
            RuleFor(r => r.Article).NotNull();
            RuleFor(r => r.LoadingOutDate).NotNull().GreaterThan(DateTimeOffset.MinValue);
            RuleFor(r => r.LoadingOutDate).NotNull().LessThan(DateTimeOffset.Now).WithMessage("Tanggal Loading Tidak Boleh Lebih dari Hari Ini");
            RuleFor(r => r.LoadingOutDate).NotNull().GreaterThan(r => r.LoadingInDate.GetValueOrDefault().Date).WithMessage(r => $"Tanggal Loading Tidak Boleh Kurang dari tanggal {r.LoadingInDate.GetValueOrDefault().ToOffset(new TimeSpan(7, 0, 0)).ToString("dd/MM/yyyy", new CultureInfo("id-ID"))}").When(r=>r.LoadingInDate!=null);
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleForEach(r => r.Items).SetValidator(new GarmentSubconLoadingOutItemValueObjectValidator());
        }
    }
}
