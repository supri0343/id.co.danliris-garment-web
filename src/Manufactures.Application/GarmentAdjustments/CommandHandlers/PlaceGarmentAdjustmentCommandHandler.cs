using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentAdjustments;
using Manufactures.Domain.GarmentAdjustments.Commands;
using Manufactures.Domain.GarmentAdjustments.Repositories;
using Manufactures.Domain.GarmentFinishedGoodStocks;
using Manufactures.Domain.GarmentFinishedGoodStocks.Repositories;
using Manufactures.Domain.GarmentFinishingIns;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GarmentSewingDOs;
using Manufactures.Domain.GarmentSewingDOs.Repositories;
using Manufactures.Domain.GarmentSewingIns;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentAdjustments.CommandHandlers
{
    public class PlaceGarmentAdjustmentCommandHandler : ICommandHandler<PlaceGarmentAdjustmentCommand, GarmentAdjustment>
    {
        private readonly IStorage _storage;
        private readonly IGarmentAdjustmentRepository _garmentAdjustmentRepository;
        private readonly IGarmentAdjustmentItemRepository _garmentAdjustmentItemRepository;
        private readonly IGarmentSewingDOItemRepository _garmentSewingDOItemRepository;
        private readonly IGarmentSewingInItemRepository _garmentSewingInItemRepository;
        private readonly IGarmentFinishingInItemRepository _garmentFinishingInItemRepository;
		private readonly IGarmentFinishedGoodStockRepository _garmentFinishedGoodStockRepository;
		private readonly IGarmentFinishedGoodStockHistoryRepository _garmentFinishedGoodStockHistoryRepository;

		public PlaceGarmentAdjustmentCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentAdjustmentRepository = storage.GetRepository<IGarmentAdjustmentRepository>();
            _garmentAdjustmentItemRepository = storage.GetRepository<IGarmentAdjustmentItemRepository>();
            _garmentSewingDOItemRepository = storage.GetRepository<IGarmentSewingDOItemRepository>();
            _garmentSewingInItemRepository = storage.GetRepository<IGarmentSewingInItemRepository>();
            _garmentFinishingInItemRepository = storage.GetRepository<IGarmentFinishingInItemRepository>();
			_garmentFinishedGoodStockRepository = storage.GetRepository<IGarmentFinishedGoodStockRepository>();
			_garmentFinishedGoodStockHistoryRepository = storage.GetRepository<IGarmentFinishedGoodStockHistoryRepository>();
		}

        public async Task<GarmentAdjustment> Handle(PlaceGarmentAdjustmentCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.ToList();
			Guid AdjustmentId = Guid.NewGuid();
		
			GarmentAdjustment garmentAdjustment = new GarmentAdjustment(
				AdjustmentId,
                GenerateAdjustmentNo(request),
                request.AdjustmentType,
                request.RONo,
                request.Article,
                new UnitDepartmentId(request.Unit.Id),
                request.Unit.Code,
                request.Unit.Name,
                request.AdjustmentDate,
                new GarmentComodityId(request.Comodity.Id),
                request.Comodity.Code,
                request.Comodity.Name,
                request.AdjustmentDesc
            );

            Dictionary<Guid, double> sewingDOItemToBeUpdated = new Dictionary<Guid, double>();
            Dictionary<Guid, double> sewingInItemToBeUpdated = new Dictionary<Guid, double>();
            Dictionary<Guid, double> finishingInItemToBeUpdated = new Dictionary<Guid, double>();
			Dictionary<Guid, double> finishedGoodItemToBeUpdated = new Dictionary<Guid, double>();
			List<GarmentFinishedGoodStock> finGoodStocks = new List<GarmentFinishedGoodStock>();

			foreach (var item in request.Items)
            {
                if (item.IsSave)
                {
					var stock = _garmentFinishedGoodStockRepository.Query.Where(x => x.Identity == item.FinishedGoodStockId).Select(s => new GarmentFinishedGoodStock(s)).Single();
					Guid AdjutmentItemId = Guid.NewGuid();
					GarmentAdjustmentItem garmentAdjustmentItem = new GarmentAdjustmentItem(
						AdjutmentItemId,
						AdjustmentId,
						item.SewingDOItemId,
						item.SewingInItemId,
						item.FinishingInItemId,
						stock.Identity,
						new SizeId(item.Size.Id),
						item.Size.Size,
						item.Product !=null ? new ProductId(item.Product.Id) : new ProductId(0),
						item.Product != null ? item.Product.Code : null,
						item.Product != null ? item.Product.Name : null,
                        item.DesignColor,
                        item.Quantity,
                        item.BasicPrice,
                        new UomId(item.Uom.Id),
                        item.Uom.Unit,
                        item.Color,
                        item.Price
                    );

                    if (request.AdjustmentType == "LOADING")
                    {
                        if (sewingDOItemToBeUpdated.ContainsKey(item.SewingDOItemId))
                        {
                            sewingDOItemToBeUpdated[item.SewingDOItemId] += item.Quantity;
                        }
                        else
                        {
                            sewingDOItemToBeUpdated.Add(item.SewingDOItemId, item.Quantity);
                        }
                    }
                    else if(request.AdjustmentType == "SEWING")
                    {
                        if (sewingInItemToBeUpdated.ContainsKey(item.SewingInItemId))
                        {
                            sewingInItemToBeUpdated[item.SewingInItemId] += item.Quantity;
                        }
                        else
                        {
                            sewingInItemToBeUpdated.Add(item.SewingInItemId, item.Quantity);
                        }
                    }
                    else if (request.AdjustmentType == "FINISHING")
                    {
                        if (finishingInItemToBeUpdated.ContainsKey(item.FinishingInItemId))
                        {
                            finishingInItemToBeUpdated[item.FinishingInItemId] += item.Quantity;
                        }
                        else
                        {
                            finishingInItemToBeUpdated.Add(item.FinishingInItemId, item.Quantity);
                        }
                    }else
					{
						if (finishedGoodItemToBeUpdated.ContainsKey(item.FinishedGoodStockId))
						{
							finishedGoodItemToBeUpdated[item.FinishedGoodStockId] += item.Quantity;
						}
						else
						{

							finishedGoodItemToBeUpdated.Add(item.FinishedGoodStockId, item.Quantity);
						}
						 
						GarmentFinishedGoodStockHistory garmentFinishedGoodStockHistory = new GarmentFinishedGoodStockHistory(
								Guid.NewGuid(),
								stock.Identity,
								Guid.Empty,
								Guid.Empty,
								Guid.Empty,
								Guid.Empty,
								AdjustmentId,
								AdjutmentItemId,
								"ADJUSTMENT",
								stock.RONo,
								stock.Article,
								stock.UnitId,
								stock.UnitCode,
								stock.UnitName,
								stock.ComodityId,
								stock.ComodityCode,
								stock.ComodityName,
								stock.SizeId,
								stock.SizeName,
								stock.UomId,
								stock.UomUnit,
								item.Quantity,
								item.BasicPrice,
								item.Price
							);
						await _garmentFinishedGoodStockHistoryRepository.Update(garmentFinishedGoodStockHistory);

					}
					await _garmentAdjustmentItemRepository.Update(garmentAdjustmentItem);
                }
            }

			if (request.AdjustmentType == "LOADING")
			{
				foreach (var sewingDOItem in sewingDOItemToBeUpdated)
				{
					var garmentSewingDOItem = _garmentSewingDOItemRepository.Query.Where(x => x.Identity == sewingDOItem.Key).Select(s => new GarmentSewingDOItem(s)).Single();
					garmentSewingDOItem.setRemainingQuantity(garmentSewingDOItem.RemainingQuantity - sewingDOItem.Value);
					garmentSewingDOItem.Modify();

					await _garmentSewingDOItemRepository.Update(garmentSewingDOItem);
				}
			}
			else if (request.AdjustmentType == "SEWING")
			{
				foreach (var sewingInItem in sewingInItemToBeUpdated)
				{
					var garmentSewingInItem = _garmentSewingInItemRepository.Query.Where(x => x.Identity == sewingInItem.Key).Select(s => new GarmentSewingInItem(s)).Single();
					garmentSewingInItem.SetRemainingQuantity(garmentSewingInItem.RemainingQuantity - sewingInItem.Value);
					garmentSewingInItem.Modify();

					await _garmentSewingInItemRepository.Update(garmentSewingInItem);
				}
			}
			else if (request.AdjustmentType == "FINISHING")
			{
				foreach (var finishingInItem in finishingInItemToBeUpdated)
				{
					var garmentFinishingInItem = _garmentFinishingInItemRepository.Query.Where(x => x.Identity == finishingInItem.Key).Select(s => new GarmentFinishingInItem(s)).Single();
					garmentFinishingInItem.SetRemainingQuantity(garmentFinishingInItem.RemainingQuantity - finishingInItem.Value);
					garmentFinishingInItem.Modify();

					await _garmentFinishingInItemRepository.Update(garmentFinishingInItem);
				}
			}
			else
			{
			
				foreach (var data in finishedGoodItemToBeUpdated)
				{
					var garmentFinishedGoodstock = _garmentFinishedGoodStockRepository.Query.Where(x => x.Identity == data.Key).Select(s => new GarmentFinishedGoodStock(s)).Single();
					garmentFinishedGoodstock.SetQuantity(garmentFinishedGoodstock.Quantity - data.Value);
					garmentFinishedGoodstock.Modify();
					await _garmentFinishedGoodStockRepository.Update(garmentFinishedGoodstock);
				}
			}

            await _garmentAdjustmentRepository.Update(garmentAdjustment);

            _storage.Save();

            return garmentAdjustment;
        }

        private string GenerateAdjustmentNo(PlaceGarmentAdjustmentCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");
            var day = now.ToString("dd");
            var unitcode = request.Unit.Code;

            var prefix = $"ADJ{unitcode}{year}{month}";
            if (request.AdjustmentType == "LOADING")
            {
                prefix = $"ADJL{unitcode}{year}{month}";
            }
            else if(request.AdjustmentType == "SEWING")
            {
                prefix = $"ADJS{unitcode}{year}{month}";
            }
            else if (request.AdjustmentType == "FINISHING")
            {
                prefix = $"ADJF{unitcode}{year}{month}";
			}
			else  
			{
				prefix = $"ADJG{unitcode}{year}{month}";
			}

			var lastAdjustmentNo = _garmentAdjustmentRepository.Query.Where(w => w.AdjustmentNo.StartsWith(prefix))
                .OrderByDescending(o => o.AdjustmentNo)
                .Select(s => int.Parse(s.AdjustmentNo.Replace(prefix, "")))
                .FirstOrDefault();
            var loadingNo = $"{prefix}{(lastAdjustmentNo + 1).ToString("D4")}";

            return loadingNo;
        }
    }
}
