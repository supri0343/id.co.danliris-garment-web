using Infrastructure.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Manufactures.Domain.GarmentLoadings.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Manufactures.Application.GarmentPreparings.Queries.GetHistoryDeletedLoadings
{
   public  class GetMonLoadHistoryDelQueryHandler : IQueryHandler<GetMonLoadHistoryDelQuery, GarmentMonLoadHistoryDelListViewModel>
    {
		private readonly IGarmentLoadingRepository garmentLoadingRepository;
		private readonly IGarmentLoadingItemRepository garmentLoadingItemRepository;

		public GetMonLoadHistoryDelQueryHandler(IStorage storage, IServiceProvider serviceProvider)
		{
			garmentLoadingRepository = storage.GetRepository<IGarmentLoadingRepository>();
			garmentLoadingItemRepository = storage.GetRepository<IGarmentLoadingItemRepository>();
		}

		public async Task<GarmentMonLoadHistoryDelListViewModel> Handle(GetMonLoadHistoryDelQuery request, CancellationToken cancellationToken)
		{
			DateTime? dateFrom = request.dateFrom;
			DateTime? dateTo = request.dateTo;

			DateTime d1 = dateFrom == null ? new DateTime(1970, 1, 1) : (DateTime)dateFrom;
			DateTime d2 = dateTo == null ? DateTime.Now : (DateTime)dateTo;

			var monLoadsHisDelQuery = from a in (from aa in  garmentLoadingRepository.Query.IgnoreQueryFilters()
											   where
												//aa.UnitId == request.unit 
												aa.DeletedDate.Value.AddHours(7).Date >= d1.Date
											   && aa.DeletedDate.Value.AddHours(7).Date <= d2.Date
											 //aa.LoadingDate.Value.AddHours(7).Date >= d1
											 //  && aa.LoadingDate.Value.AddHours(7).Date <= d2
											// && aa.Deleted == false
												 && aa.Deleted == true
												 select new
											   {
												   aa.Identity,
												   aa.Article,
												   aa.RONo,
												   aa.UnitName,
												   aa.DeletedBy,
												   aa.DeletedDate,
													 aa.LoadingNo,
													 aa.LoadingDate,
													 aa.SewingDONo,
													 aa.ComodityName

												 })
									  join b in garmentLoadingItemRepository.Query.IgnoreQueryFilters() on a.Identity equals b.LoadingId
									  select new
									{
										  deletedBys = a.DeletedBy, // DeletedBy
										  deletedDates = a.DeletedDate, // DeletedDate
										  loadingNos = a.LoadingNo, // LoadingNo
										  loadingDates = a.LoadingDate, // LoadingDate
										  unitNames = a.UnitName, // UnitName
										  roNos = a.RONo, // RONo
										  sewingDoNos = a.SewingDONo, // SewingDONo
										  commodityNames = a.ComodityName, // ComodityName
											productCodes = b.ProductCode, // ProductCode
											sizeNames = b.SizeName, // SizeName
											quantities = b.Quantity, // Quantity
											colors = b.Color// Color
									};

			GarmentMonLoadHistoryDelListViewModel garmentMonLoadHistoryDelListViewModel = new GarmentMonLoadHistoryDelListViewModel();
			List<GarmentMonLoadHistoryDelDto> monLoadsHisDelDtos = new List<GarmentMonLoadHistoryDelDto>();
			foreach (var item in monLoadsHisDelQuery)
			{
				GarmentMonLoadHistoryDelDto garmentMonLoadHistoryDelDto = new GarmentMonLoadHistoryDelDto()
				{
					deletedBys = item.deletedBys,
					deletedDates = item.deletedDates,
					loadingNos = item.loadingNos,
					loadingDates = item.loadingDates,
					unitNames = item.unitNames,
					roNos = item.roNos,
					sewingDoNos = item.sewingDoNos,
					commodityNames = item.commodityNames,
					productCodes = item.productCodes,
					sizeNames = item.sizeNames,
					quantities = item.quantities,
					colors = item.colors
				};
				monLoadsHisDelDtos.Add(garmentMonLoadHistoryDelDto);
			}
			garmentMonLoadHistoryDelListViewModel.garmentMonitorings = monLoadsHisDelDtos;
			return garmentMonLoadHistoryDelListViewModel;
		}
	}
}

