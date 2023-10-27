using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentPreparings.Repositories;
using System; 
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Manufactures.Application.GarmentCuttingIns.Queries.GetHistoryDeleted
{
	public class GetMonCuttingHistoryDelQueryHandler : IQueryHandler<GetMonCuttingHistoryDelQuery, GarmentMonCuttingHistoryDelViewModel>
	{
		protected readonly IHttpClientService _http;
		private readonly IStorage _storage; 
		private readonly IGarmentCuttingInRepository garmentCuttingInRepository;
		private readonly IGarmentCuttingInItemRepository garmentCuttingInItemRepository;
		private readonly IGarmentCuttingInDetailRepository garmentCuttingInDetailRepository;

		public GetMonCuttingHistoryDelQueryHandler(IStorage storage, IServiceProvider serviceProvider)
		{
			_storage = storage;
			garmentCuttingInRepository = storage.GetRepository<IGarmentCuttingInRepository>();
			garmentCuttingInItemRepository = storage.GetRepository<IGarmentCuttingInItemRepository>();
			garmentCuttingInDetailRepository = storage.GetRepository<IGarmentCuttingInDetailRepository>();
			
			_http = serviceProvider.GetService<IHttpClientService>();
        }

        public async Task<GarmentMonCuttingHistoryDelViewModel> Handle(GetMonCuttingHistoryDelQuery request, CancellationToken cancellationToken)
        {
			DateTime? dateFrom = request.dateFrom;
			DateTime? dateTo = request.dateTo;

			DateTime d1 = dateFrom == null ? new DateTime(1970, 1, 1) : (DateTime)dateFrom;
			DateTime d2 = dateTo == null ? DateTime.Now : (DateTime)dateTo;

			var query = from aa in garmentCuttingInRepository.Query.IgnoreQueryFilters()
						join bb in garmentCuttingInItemRepository.Query.IgnoreQueryFilters() on aa.Identity equals bb.CutInId
						join cc in garmentCuttingInDetailRepository.Query.IgnoreQueryFilters() on bb.Identity equals cc.CutInItemId
						where
						 aa.Deleted == true &&
						 aa.DeletedDate.Value.Date >= d1.Date
						&& aa.DeletedDate.Value.Date <= d2.Date
						select new
						{
							RO = aa.RONo,
							CuttingInDate= aa.CuttingInDate,
							CuttingInNo= aa.CutInNo,
							CutInDate= aa.CuttingInDate,
							CuttingInType = aa.CuttingType,
							CuttingFrom = aa.CuttingFrom,
							ProductCode = cc.ProductCode,
							PreparingQuantity = cc.PreparingQuantity,
							PreparingUomUnit = cc.PreparingUomUnit,
							CuttingInQuantity = cc.CuttingInQuantity ,
							RemainingQuantity = cc.RemainingQuantity ,
							CuttingInUomUnit = cc.CuttingInUomUnit,
							BasicPrice = cc.BasicPrice,
							Unit = aa.UnitName,
							FC= aa.FC,
							DeletedBy= cc.DeletedBy,
							DeletedDate= cc.DeletedDate
						};
		 

			GarmentMonCuttingHistoryDelViewModel garmentMonCuttingHistoryViewModel = new GarmentMonCuttingHistoryDelViewModel();
			List<GarmentMonCuttingHistoryDelDto> monPreHistoryDelDtos = new List<GarmentMonCuttingHistoryDelDto>();
			//List<monPreHistoryDelViewTemp> ShowUp = new List<monPreHistoryDelViewTemp>();
			foreach (var item in query)
			{
				GarmentMonCuttingHistoryDelDto delDto = new GarmentMonCuttingHistoryDelDto()
				{

					RO = item.RO,
					cuttingInQty = item.CuttingInQuantity,
					cuttingFrom = item.CuttingFrom,
					cuttingInType = item.CuttingInType,
					cuttingInDate = item.CutInDate,
					cuttingInNo = item.CuttingInNo,
					remainingQty = item.RemainingQuantity,
					itemCode = item.ProductCode,
					unit = item.Unit,
					deletedBy = item.DeletedBy,
					deletedDate = item.DeletedDate,
					fc = item.FC,
					preparingQty = item.PreparingQuantity,
					uomUnitCuttingIn = item.CuttingInUomUnit,
					uomUnitsPrep = item.PreparingUomUnit,
					basicPrice = item.BasicPrice

				};
				monPreHistoryDelDtos.Add(delDto);
			}
			garmentMonCuttingHistoryViewModel.garmentMonitorings = monPreHistoryDelDtos;
			return garmentMonCuttingHistoryViewModel;
		}
	}
}
