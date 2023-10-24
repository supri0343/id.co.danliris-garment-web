using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace Manufactures.Application.GarmentCuttingOuts.Queries.GetMonitoringDeleteHistory
{
	public class GetMonCuttingOutHistoryDelQueryHandler : IQueryHandler<GetMonCuttingOutHistoryDelQuery, GarmentMonCuttingOutHistoryDelViewModel>
	{
		protected readonly IHttpClientService _http;
		private readonly IStorage _storage;
		private readonly IGarmentCuttingOutRepository garmentCuttingOutRepository;
		private readonly IGarmentCuttingOutItemRepository garmentCuttingOutItemRepository;
		private readonly IGarmentCuttingOutDetailRepository garmentCuttingOutDetailRepository;

		public GetMonCuttingOutHistoryDelQueryHandler(IStorage storage, IServiceProvider serviceProvider)
		{
			_storage = storage;
			garmentCuttingOutRepository = storage.GetRepository<IGarmentCuttingOutRepository>();
			garmentCuttingOutItemRepository = storage.GetRepository<IGarmentCuttingOutItemRepository>();
			garmentCuttingOutDetailRepository = storage.GetRepository<IGarmentCuttingOutDetailRepository>();

			_http = serviceProvider.GetService<IHttpClientService>();
		}

        public async Task<GarmentMonCuttingOutHistoryDelViewModel> Handle(GetMonCuttingOutHistoryDelQuery request, CancellationToken cancellationToken)
        {
			DateTime? dateFrom = request.dateFrom;
			DateTime? dateTo = request.dateTo;

			DateTime d1 = dateFrom == null ? new DateTime(1970, 1, 1) : (DateTime)dateFrom;
			DateTime d2 = dateTo == null ? DateTime.Now : (DateTime)dateTo;

			var query = from aa in garmentCuttingOutRepository.Query.IgnoreQueryFilters()
						join bb in garmentCuttingOutItemRepository.Query.IgnoreQueryFilters() on aa.Identity equals bb.CutOutId
						join cc in garmentCuttingOutDetailRepository.Query.IgnoreQueryFilters() on bb.Identity equals cc.CutOutItemId
						where
						 aa.Deleted == true &&
						 aa.DeletedDate.Value.Date >= d1.Date
						&& aa.DeletedDate.Value.Date <= d2.Date
						select new
						{
							RO = aa.RONo,
							CuttingOutDate = aa.CuttingOutDate,
							CuttingOutNo = aa.CutOutNo,
							Comodity = aa.ComodityName,
							ProductCode = bb.ProductCode,
							TotalCuttingOut = bb.TotalCuttingOut,
							Color = cc.Color,
							SizeName = cc.SizeName,
							CuttingOutQty = cc.CuttingOutQuantity,
							CuttingOutUomUnit = cc.CuttingOutUomUnit,
							BasicPrice = cc.BasicPrice,
							Unit = aa.UnitName,
							DeletedBy = cc.DeletedBy,
							DeletedDate = cc.DeletedDate,
							Id = aa.Identity
						};


			GarmentMonCuttingOutHistoryDelViewModel garmentMonCuttingHistoryViewModel = new GarmentMonCuttingOutHistoryDelViewModel();
			List<GarmentMonCuttingOutHistoryDelDto> monPreHistoryDelDtos = new List<GarmentMonCuttingOutHistoryDelDto>();
			//List<monPreHistoryDelViewTemp> ShowUp = new List<monPreHistoryDelViewTemp>();
			foreach (var item in query)
			{
				GarmentMonCuttingOutHistoryDelDto delDto = new GarmentMonCuttingOutHistoryDelDto()
				{

					RO = item.RO,
					cutOutNo = item.CuttingOutNo,
					cuttingOutDate = item.CuttingOutDate,
					comodityCode = item.Comodity,
					SizeName = item.SizeName,
					totalCuttingOut = item.TotalCuttingOut,
					cuttingOutUomUnit = item.CuttingOutUomUnit,
					itemCode = item.ProductCode,
					unit = item.Unit,
					deletedBy = item.DeletedBy,
					deletedDate = item.DeletedDate,
					color= item.Color,
					Id= item.Id
					
				};
				monPreHistoryDelDtos.Add(delDto);
			}
			garmentMonCuttingHistoryViewModel.garmentMonitorings = monPreHistoryDelDtos;
			return garmentMonCuttingHistoryViewModel;
		}
    }
}
