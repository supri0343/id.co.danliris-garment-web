using ExtCore.Data.Abstractions;
using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList.Repositories;

namespace Manufactures.Domain.GarmentSubcon.InvoicePackingList.Commands
{
    public class PlaceGarmentSubconInvoicePackingListCommand : ICommand<SubconInvoicePackingList>
    {
        public string InvoiceNo { get; set; }
        public string BCType { get; set; }
        public DateTimeOffset Date { get; set; }
        public Supplier Supplier { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public string SupplierAddress { get; set; }
        public string ContractNo { get; set; }
        public double NW { get; set; }
        public double GW { get; set; }
        public string Remark { get; set; }
        public string BuyerStaff { get; set; }
        public Guid SubconContractId { get; set; }
        public List<SubconInvoicePackingListItemValueObject> Items { get; set; }
    }
    public class PlaceGarmentSubconInvoicePackingListCommandValidator : AbstractValidator<PlaceGarmentSubconInvoicePackingListCommand>
    {
        public PlaceGarmentSubconInvoicePackingListCommandValidator()
        {
            RuleFor(r => r.BCType).NotNull();
            RuleFor(r => r.Date).NotNull().GreaterThan(DateTimeOffset.MinValue).WithMessage("Tanggal Tidak Boleh Kosong");
            RuleFor(r => r.Supplier).NotNull();
            RuleFor(r => r.Supplier.Id).NotEmpty().OverridePropertyName("Supplier").When(w => w.Supplier != null);
            RuleFor(r => r.ContractNo).NotNull();
            RuleFor(r => r.NW).NotNull();
            RuleFor(r => r.GW).NotNull();
            RuleForEach(r => r.Items).SetValidator(r => new SubconInvoicePackingListItemValueObjectValidator(r)).When(s => s.Items != null);
        }
    }

    public class SubconInvoicePackingListItemValueObjectValidator : AbstractValidator<SubconInvoicePackingListItemValueObject>
    {
		public SubconInvoicePackingListItemValueObjectValidator(PlaceGarmentSubconInvoicePackingListCommand placeGarmentSubconInvoicePackingListCommand)
        {
            RuleFor(r => r.DLNo).NotNull();
            RuleFor(r => r.DLNo).Must((dlno) =>
            {
                return placeGarmentSubconInvoicePackingListCommand.Items.FindAll(a => a.DLNo != null && a.DLNo == dlno).Count < 2;
            }).WithMessage("No SJ sudah ada");
            //.OverridePropertyName("DLNo")
            //.When(c => c.DLNo != null);

            //RuleFor(r => r.Product)
            //    .NotNull()
            //    .WithMessage("Barang harus diisi.");

            //RuleFor(r => r.Product.Id).NotEmpty().OverridePropertyName("Product").When(w => w.Product != null)
            //    .WithMessage("Barang harus diisi.");
            //RuleFor(r => r.Uom)
            //    .NotNull()
            //    .WithMessage("Satuan harus diisi.");

            //RuleFor(r => r.Uom.Id).NotEmpty().OverridePropertyName("Uom").When(w => w.Uom != null)
            //    .WithMessage("Satuan harus diisi.");

            RuleFor(r => r.Quantity)
                .GreaterThan(0)
                .WithMessage("Jumlah harus lebih dari '0'.");
        }
    }
}
