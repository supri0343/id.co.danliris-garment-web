using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Domain.GarmentPreparings.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.External.DanLirisClient.Microservice;
using Newtonsoft.Json;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;
using ExtCore.Data.Abstractions;
using Manufactures.Domain.GarmentPreparings.ReadModels;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.ExpenditureROResult;
using Microsoft.Extensions.DependencyInjection;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentAvalProducts.Repositories;
using Manufactures.Domain.GarmentDeliveryReturns.Repositories;
using System.IO;
using System.Data;
using OfficeOpenXml;
using Microsoft.EntityFrameworkCore;

namespace Manufactures.Application.GarmentPreparings.Queries.GetHistoryDeleted
{
    public class GetMonPreHistoryDelQueryHandler :IQueryHandler <GetMonPreHistoryDelQuery, GarmentMonPreHistoryDelViewModel>
    {
		protected readonly IHttpClientService _http;
		private readonly IStorage _storage;

		private readonly IGarmentPreparingRepository garmentPreparingRepository;
		private readonly IGarmentPreparingItemRepository garmentPreparingItemRepository;
		private readonly IGarmentCuttingInRepository garmentCuttingInRepository;
		private readonly IGarmentCuttingInItemRepository garmentCuttingInItemRepository;
		private readonly IGarmentCuttingInDetailRepository garmentCuttingInDetailRepository;
		private readonly IGarmentAvalProductRepository garmentAvalProductRepository;
		private readonly IGarmentAvalProductItemRepository garmentAvalProductItemRepository;
		private readonly IGarmentDeliveryReturnRepository garmentDeliveryReturnRepository;
		private readonly IGarmentDeliveryReturnItemRepository garmentDeliveryReturnItemRepository;

		public GetMonPreHistoryDelQueryHandler(IStorage storage, IServiceProvider serviceProvider)
		{
			_storage = storage;
			garmentPreparingRepository = storage.GetRepository<IGarmentPreparingRepository>();
			garmentPreparingItemRepository = storage.GetRepository<IGarmentPreparingItemRepository>();
			garmentCuttingInRepository = storage.GetRepository<IGarmentCuttingInRepository>();
			garmentCuttingInItemRepository = storage.GetRepository<IGarmentCuttingInItemRepository>();
			garmentCuttingInDetailRepository = storage.GetRepository<IGarmentCuttingInDetailRepository>();
			garmentAvalProductRepository = storage.GetRepository<IGarmentAvalProductRepository>();
			garmentAvalProductItemRepository = storage.GetRepository<IGarmentAvalProductItemRepository>();
			garmentDeliveryReturnRepository = storage.GetRepository<IGarmentDeliveryReturnRepository>();
			garmentDeliveryReturnItemRepository = storage.GetRepository<IGarmentDeliveryReturnItemRepository>();
			_http = serviceProvider.GetService<IHttpClientService>();
		}


		class monPreHistoryDelViewTemp
        {
			public Guid Id { get;  set; }
			public string deletedBy { get; set; }
			public string buyer { get; set; }
		    public string RO { get; set; }
			public string articles { get; set; }
			public string detailExpend { get; set; }
			public string processDates { get; set; }
			public string uenNO { get; set; }
			public string unit { get; set; }
			public string deletedDate { get; set; }
			public string buyesNamed { get; set; }
			public string produkCodes { get; set; }
			public string kuantityes { get; set; }
			public string uomUnits { get; set; }
			public string basicPrieces { get; set; }
		}

		public async Task<GarmentMonPreHistoryDelViewModel> Handle(GetMonPreHistoryDelQuery request, CancellationToken cancellationToken)
        {
			DateTime? dateFrom = request.dateFrom;
			DateTime? dateTo =request.dateTo;

			DateTime d1 = dateFrom == null ? new DateTime(1970, 1, 1) : (DateTime)dateFrom;
			DateTime d2 = dateTo == null ? DateTime.Now : (DateTime)dateTo;

			var monpreHisDelQuery = from a in (from aa in garmentPreparingRepository.Query.IgnoreQueryFilters()
											   where 
											   //aa.UnitId == request.unit 
											    aa.ProcessDate.Value.AddHours(7).Date >= d1
											   && aa.ProcessDate.Value.AddHours(7).Date <= d2

										 && aa.Deleted==true
											   select new
											   {
												   aa.Identity,
												   aa.Article,
												   aa.BuyerCode,
												   aa.RONo,
												   aa.ProcessDate,
												   aa.UnitName,
												   aa.DeletedBy,
												   aa.DeletedDate,
												   aa.BuyerName,
												   aa.UENNo
											   })
									join b in garmentPreparingItemRepository.Query.IgnoreQueryFilters()  on a.Identity equals b.GarmentPreparingId
									select new 
									{
										buyer = a.BuyerCode,
										RO = a.RONo,
										articles = a.Article,
										Id = a.Identity,
										detailExpend = b.UENItemId,
										processDates = a.ProcessDate,

										uenNO=a.UENNo,
										unit =a.UnitName,
										deletedBy=a.DeletedBy,
										deletedDate=a.DeletedDate,
										buyesNamed=a.BuyerName,
										produkCodes=b.ProductCode,
										kuantityes=b.Quantity,
										uomUnits=b.UomUnit,
										basicPrieces=b.BasicPrice
									};

			GarmentMonPreHistoryDelViewModel garmentMonPreHistoryDelViewModel = new GarmentMonPreHistoryDelViewModel();
			List<GarmentMonPreHistoryDelDto> monPreHistoryDelDtos = new List<GarmentMonPreHistoryDelDto>();
			//List<monPreHistoryDelViewTemp> ShowUp = new List<monPreHistoryDelViewTemp>();
			foreach (var item in monpreHisDelQuery)
			{
				GarmentMonPreHistoryDelDto garmentMonPreHistoryDelDto = new GarmentMonPreHistoryDelDto()
				{

					buyer = item.buyer,
					articles = item.articles,
					Id = item.Id,
					detailExpend = item.detailExpend,

					processDates = item.processDates,
					RO = item.RO,
					uenNO = item.uenNO,
					unit = item.unit,
					deletedBy = item.deletedBy,
					deletedDate = item.deletedDate,
					buyesNamed = item.buyesNamed,
					produkCodes = item.produkCodes,
					kuantityes = item.kuantityes,
					uomUnits = item.uomUnits,
					basicPrieces = item.basicPrieces

				};
				monPreHistoryDelDtos.Add(garmentMonPreHistoryDelDto);
			}
			garmentMonPreHistoryDelViewModel.garmentMonitorings = monPreHistoryDelDtos;
			return garmentMonPreHistoryDelViewModel;
		}
	}
}
