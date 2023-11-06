using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods.Commands;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods.Repositories;
using Manufactures.Domain.LogHistory;
using Manufactures.Domain.LogHistory.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.SampleExpenditureGoods.CommandHandlers
{
    public class UpdateGarmentSampleExpenditureGoodCommandHandler : ICommandHandler<UpdateGarmentSampleExpenditureGoodCommand, GarmentSampleExpenditureGood>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSampleExpenditureGoodRepository _GarmentSampleExpenditureGoodRepository;
        private readonly IGarmentSampleExpenditureGoodItemRepository _GarmentSampleExpenditureGoodItemRepository;
        //----------
        private readonly ILogHistoryRepository _logHistoryRepository;
        //-------
        public UpdateGarmentSampleExpenditureGoodCommandHandler(IStorage storage)
        {
            _storage = storage;
            _GarmentSampleExpenditureGoodRepository = storage.GetRepository<IGarmentSampleExpenditureGoodRepository>();
            _GarmentSampleExpenditureGoodItemRepository = storage.GetRepository<IGarmentSampleExpenditureGoodItemRepository>();
            //------------
            _logHistoryRepository = storage.GetRepository<ILogHistoryRepository>();
            //------------
        }

        public async Task<GarmentSampleExpenditureGood> Handle(UpdateGarmentSampleExpenditureGoodCommand request, CancellationToken cancellationToken)
        {
            var ExpenditureGood = _GarmentSampleExpenditureGoodRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSampleExpenditureGood(o)).Single();

            _GarmentSampleExpenditureGoodItemRepository.Find(o => o.ExpenditureGoodId == ExpenditureGood.Identity).ForEach(async expenditureItem =>
            {
                await _GarmentSampleExpenditureGoodItemRepository.Update(expenditureItem);
            });
            ExpenditureGood.SetCarton(request.Carton);
            ExpenditureGood.SetExpenditureDate(request.ExpenditureDate);
            ExpenditureGood.SetPackingListId(request.PackingListId);
            ExpenditureGood.SetInvoice(request.Invoice);
            ExpenditureGood.SetIsReceived(request.IsReceived);
            ExpenditureGood.Modify();
            await _GarmentSampleExpenditureGoodRepository.Update(ExpenditureGood);
            //disini
            //Add Log History
            LogHistory logHistory = new LogHistory(new Guid(), "PRODUKSI PENGELUARAN BARANG JADI SAMPLE", "Update Pengeluaran Barang Jadi Sample - " + ExpenditureGood.ExpenditureGoodNo, DateTime.Now);
            await _logHistoryRepository.Update(logHistory);
            //-----------

            _storage.Save();

            return ExpenditureGood;
        }
    }
}
