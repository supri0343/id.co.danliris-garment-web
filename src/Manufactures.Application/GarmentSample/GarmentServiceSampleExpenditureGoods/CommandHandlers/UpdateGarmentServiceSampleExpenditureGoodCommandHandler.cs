using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentFinishedGoodStocks.Repositories;
using Manufactures.Domain.GarmentSample.SampleFinishedGoodStocks.Repositories;
using Manufactures.Domain.GarmentSample.ServiceSampleExpenditureGood;
using Manufactures.Domain.GarmentSample.ServiceSampleExpenditureGood.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleExpenditureGood.Repositories;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.Repositories;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Application.GarmentSample.GarmentServiceSampleExpenditureGoods.CommandHandlers
{
    public class UpdateGarmentServiceSampleExpenditureGoodCommandHandler : ICommandHandler<UpdateGarmentServiceSampleExpenditureGoodCommand, GarmentServiceSampleExpenditureGood>
    {
        private readonly IStorage _storage;
        private readonly IGarmentServiceSampleExpenditureGoodRepository _garmentServiceSampleExpenditureGoodRepository;
        private readonly IGarmentServiceSampleExpenditureGoodtemRepository _garmentServiceSampleExpenditureGoodItemRepository;

        private readonly IGarmentSampleFinishedGoodStockRepository _garmentFinishedGoodStockRepository;
        private readonly IGarmentSampleFinishedGoodStockHistoryRepository _garmentFinishedGoodStockHistoryRepository;
        private readonly IGarmentComodityPriceRepository _garmentComodityPriceRepository;

        public UpdateGarmentServiceSampleExpenditureGoodCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentServiceSampleExpenditureGoodRepository = storage.GetRepository<IGarmentServiceSampleExpenditureGoodRepository>();
            _garmentServiceSampleExpenditureGoodItemRepository = storage.GetRepository<IGarmentServiceSampleExpenditureGoodtemRepository>();

            _garmentComodityPriceRepository = storage.GetRepository<IGarmentComodityPriceRepository>();
            _garmentFinishedGoodStockRepository = storage.GetRepository<IGarmentSampleFinishedGoodStockRepository>();
            _garmentFinishedGoodStockHistoryRepository = storage.GetRepository<IGarmentSampleFinishedGoodStockHistoryRepository>();
        }

        public async Task<GarmentServiceSampleExpenditureGood> Handle(UpdateGarmentServiceSampleExpenditureGoodCommand request, CancellationToken cancellationToken)
        {
           var SampleExpenditureGood = _garmentServiceSampleExpenditureGoodRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentServiceSampleExpenditureGood(o)).Single();

            _garmentServiceSampleExpenditureGoodItemRepository.Find(s => s.ServiceSampleExpenditureGoodId == request.Identity).ForEach(async SampleExpenditureItem =>
            {
                foreach (var item in request.Items)
                {
                    if (SampleExpenditureItem.Identity == item.Id)
                    {
                        SampleExpenditureItem.SetQuantity(item.Quantity);
                        SampleExpenditureItem.Modify();
                        await _garmentServiceSampleExpenditureGoodItemRepository.Update(SampleExpenditureItem);
                    }
                }
            });
            SampleExpenditureGood.SetDate(request.ServiceSubconExpenditureGoodDate.GetValueOrDefault());
            SampleExpenditureGood.SetBuyerId(new BuyerId(request.Buyer.Id));
            SampleExpenditureGood.SetBuyerCode(request.Buyer.Code);
            SampleExpenditureGood.SetBuyerName(request.Buyer.Name);
            SampleExpenditureGood.SetQtyPacking(request.QtyPacking);
            SampleExpenditureGood.SetUomUnit(request.UomUnit);
            SampleExpenditureGood.Modify();
            await _garmentServiceSampleExpenditureGoodRepository.Update(SampleExpenditureGood);

            _storage.Save();

            return SampleExpenditureGood;
        }
    }
}
