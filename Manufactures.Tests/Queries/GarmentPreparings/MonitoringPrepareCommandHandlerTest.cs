using Barebone.Tests;
using DanLiris.Admin.Web;
using ExtCore.Data.Abstractions;
using FluentAssertions;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Application.GarmentPreparings.CommandHandlers;
using Manufactures.Application.GarmentPreparings.Queries.GetHistoryDeleted;
using Manufactures.Application.GarmentPreparings.Queries.GetMonitoringPrepare;
using Manufactures.Domain.GarmentAvalProducts;
using Manufactures.Domain.GarmentAvalProducts.ReadModels;
using Manufactures.Domain.GarmentAvalProducts.Repositories;
using Manufactures.Domain.GarmentAvalProducts.ValueObjects;
using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GarmentCuttingIns.ReadModels;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentDeliveryReturns;
using Manufactures.Domain.GarmentDeliveryReturns.ReadModels;
using Manufactures.Domain.GarmentDeliveryReturns.Repositories;
using Manufactures.Domain.GarmentPreparings;
using Manufactures.Domain.GarmentPreparings.ReadModels;
using Manufactures.Domain.GarmentPreparings.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.GarmentPreparings.Commands;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.ExpenditureROResult;

namespace Manufactures.Tests.Queries.GarmentPreparings
{
	public class MonitoringPrepareCommandHandlerTest : BaseCommandUnitTest
	{
		private readonly Mock<IGarmentPreparingRepository> _mockGarmentPreparingRepository;
		private readonly Mock<IGarmentPreparingItemRepository> _mockGarmentPreparingItemRepository;
		private readonly Mock<IGarmentCuttingInRepository> _mockGarmentCuttingInRepository;
		private readonly Mock<IGarmentCuttingInItemRepository> _mockGarmentCuttingInItemRepository;
		private readonly Mock<IGarmentCuttingInDetailRepository> _mockGarmentCuttingInDetailRepository;
		private readonly Mock<IGarmentAvalProductRepository> _mockGarmentAvalProductRepository;
		private readonly Mock<IGarmentAvalProductItemRepository> _mockGarmentAvalProductItemRepository;
		private readonly Mock<IGarmentDeliveryReturnRepository> _mockGarmentDeliveryReturnRepository;
		private readonly Mock<IGarmentDeliveryReturnItemRepository> _mockGarmentDeliveryReturnItemRepository;
		protected readonly Mock<IHttpClientService> _mockhttpService;
		private Mock<IServiceProvider> serviceProviderMock;
	
		public MonitoringPrepareCommandHandlerTest()
		 {
			_mockGarmentPreparingRepository = CreateMock<IGarmentPreparingRepository>();
			_mockGarmentPreparingItemRepository = CreateMock<IGarmentPreparingItemRepository>();
			_MockStorage.SetupStorage(_mockGarmentPreparingRepository);
			_MockStorage.SetupStorage(_mockGarmentPreparingItemRepository);

			_mockGarmentCuttingInRepository = CreateMock<IGarmentCuttingInRepository>();
			_mockGarmentCuttingInItemRepository = CreateMock<IGarmentCuttingInItemRepository>();
			_mockGarmentCuttingInDetailRepository = CreateMock<IGarmentCuttingInDetailRepository>();
			_MockStorage.SetupStorage(_mockGarmentCuttingInRepository);
			_MockStorage.SetupStorage(_mockGarmentCuttingInItemRepository);
			_MockStorage.SetupStorage(_mockGarmentCuttingInDetailRepository);

			_mockGarmentAvalProductRepository = CreateMock<IGarmentAvalProductRepository>();
			_mockGarmentAvalProductItemRepository = CreateMock<IGarmentAvalProductItemRepository>();
			_MockStorage.SetupStorage(_mockGarmentAvalProductRepository);
			_MockStorage.SetupStorage(_mockGarmentAvalProductItemRepository);

			_mockGarmentDeliveryReturnRepository = CreateMock<IGarmentDeliveryReturnRepository>();
			_mockGarmentDeliveryReturnItemRepository = CreateMock<IGarmentDeliveryReturnItemRepository>();
			_MockStorage.SetupStorage(_mockGarmentDeliveryReturnRepository);
			_MockStorage.SetupStorage(_mockGarmentDeliveryReturnItemRepository);

			serviceProviderMock = new Mock<IServiceProvider>();
			_mockhttpService = CreateMock<IHttpClientService>();
			
			ExpenditureROViewModel expenditureROViewModel = new ExpenditureROViewModel
			{
				DetailExpenditureId=1,
				ROAsal="ROAsal"
			};
			//_mockhttpService.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<string>()))
			//	.ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{\"data\": "+ JsonConvert.SerializeObject(expenditureROViewModel) + "}") });
			//serviceProviderMock.Setup(x => x.GetService(typeof(IHttpClientService))).Returns(_mockhttpService.Object);
			
		}

		private GetMonitoringPrepareQueryHandler CreateGetMonitoringPrepareQueryHandler()
		{
			return new GetMonitoringPrepareQueryHandler(_MockStorage.Object, serviceProviderMock.Object);
		}

		private GetMonPreHistoryDelQueryHandler CreateGetMonPreHistoryDelQueryHandler()
        {
			return new GetMonPreHistoryDelQueryHandler(_MockStorage.Object, serviceProviderMock.Object);
		}

		[Fact]
		public async Task Handle_StateUnderTest_ExpectedBehavior()
		{
			 
			GetMonitoringPrepareQueryHandler unitUnderTest = CreateGetMonitoringPrepareQueryHandler();
			CancellationToken cancellationToken = CancellationToken.None;

			Guid guidPrepare = Guid.NewGuid();
			Guid guidPrepareItem = Guid.NewGuid();
			Guid guidCuttingIn = Guid.NewGuid();
			Guid guidCuttingInItem = Guid.NewGuid();
			Guid guidCuttingInDetail = Guid.NewGuid();
			Guid guidAvalProduct = Guid.NewGuid();
			Guid guidAvalProductItem = Guid.NewGuid();
			Guid guidDeliveryReturn = Guid.NewGuid();
			Guid guidDeliveryReturnItem = Guid.NewGuid();
			GetMonitoringPrepareQuery getXlsPrepareQuery = new GetMonitoringPrepareQuery(1, 25, "{}", 1, DateTime.Now, DateTime.Now.AddDays(2), "token");

			_mockGarmentPreparingItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentPreparingItemReadModel>
				{
					new Domain.GarmentPreparings.GarmentPreparingItem(guidPrepareItem, 1, new Domain.GarmentPreparings.ValueObjects.ProductId(1), "", "", "", 0, new Domain.GarmentPreparings.ValueObjects.UomId(1), "", "", 0, 50, guidPrepare,null).GetReadModel()
				}.AsQueryable());

			_mockGarmentPreparingRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentPreparingReadModel>
				{
					new Domain.GarmentPreparings.GarmentPreparing(guidPrepare,1,"",new Domain.GarmentPreparings.ValueObjects.UnitDepartmentId(1),"","",DateTimeOffset.Now,"roNo","",true,new BuyerId(1), null,null).GetReadModel()
				}.AsQueryable());

			_mockGarmentCuttingInItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentCuttingInItemReadModel>
				{
					new GarmentCuttingInItem(guidCuttingInItem,guidCuttingIn,guidPrepare,1,"",Guid.Empty,null).GetReadModel()
				}.AsQueryable());

			_mockGarmentCuttingInRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentCuttingInReadModel>
				{
					new GarmentCuttingIn(guidCuttingIn,"","Main Fabric","","","",new UnitDepartmentId(1),"","",DateTimeOffset.Now,4.5).GetReadModel()
				}.AsQueryable());

			_mockGarmentCuttingInDetailRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentCuttingInDetailReadModel>
				{
					new GarmentCuttingInDetail(guidCuttingInDetail,guidCuttingInItem,guidPrepareItem,Guid.Empty,Guid.Empty,new Domain.Shared.ValueObjects.ProductId(1),"","","","",9,new Domain.Shared.ValueObjects.UomId(1),"",4,new Domain.Shared.ValueObjects.UomId(1),"",1,100,100,5.5,null).GetReadModel()
				}.AsQueryable());

			_mockGarmentAvalProductItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentAvalProductItemReadModel>
				{
					new GarmentAvalProductItem(guidAvalProductItem,guidAvalProduct,new GarmentPreparingId(guidPrepare.ToString()),new GarmentPreparingItemId(guidPrepareItem.ToString()),new Domain.GarmentAvalProducts.ValueObjects.ProductId(1),"","","",9,new Domain.GarmentAvalProducts.ValueObjects.UomId(1),"",1,false,"",DateTime.Now,"","").GetReadModel()
				}.AsQueryable());
			_mockGarmentAvalProductRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentAvalProductReadModel>
				{
					new GarmentAvalProduct(guidAvalProduct,"","",DateTimeOffset.Now,new UnitDepartmentId (1),"","").GetReadModel()
				}.AsQueryable());
			_mockGarmentDeliveryReturnItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentDeliveryReturnItemReadModel>
				{
					new GarmentDeliveryReturnItem(guidDeliveryReturnItem,guidDeliveryReturn,1,1,guidPrepareItem.ToString(), new Domain.GarmentDeliveryReturns.ValueObjects.ProductId(1),"","","","",9, new Domain.GarmentDeliveryReturns.ValueObjects.UomId(1),"","color","rak","level","box","area").GetReadModel()
				}.AsQueryable());
			_mockGarmentDeliveryReturnRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentDeliveryReturnReadModel>
				{
					 new GarmentDeliveryReturn(guidDeliveryReturn, "", "", "", 1, "", 1, guidPrepare.ToString(), DateTimeOffset.Now, "", new Domain.GarmentDeliveryReturns.ValueObjects.UnitDepartmentId(1), "", "", new Domain.GarmentDeliveryReturns.ValueObjects.StorageId(1), "", "", true).GetReadModel()
				}.AsQueryable());

			 
			var result = await unitUnderTest.Handle(getXlsPrepareQuery, cancellationToken);

			 
			result.Should().NotBeNull();
		}

        //=====----->mdp unit test history deleted prepare Handler <----====//
        [Fact]
		public async Task Handle_MonHisDeleted_StateUnderTest_ExpectedBehavior_NULLdATE()
		{

			GetMonPreHistoryDelQueryHandler unitUnderTest = CreateGetMonPreHistoryDelQueryHandler();
			CancellationToken cancellationToken = CancellationToken.None;

			Guid guidPrepare = Guid.NewGuid();
			Guid guidPrepareItem = Guid.NewGuid();

			//GetMonPreHistoryDelQuery getHendlerMonHisDelPreQuery = new GetMonPreHistoryDelQuery(null, DateTime.Now, DateTime.Now);
			GetMonPreHistoryDelQuery getHendlerMonHisDelPreQuery = new GetMonPreHistoryDelQuery(null, null,null);

			_mockGarmentPreparingItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentPreparingItemReadModel>
				{
					new Domain.GarmentPreparings.GarmentPreparingItem(guidPrepareItem, 1, new Domain.GarmentPreparings.ValueObjects.ProductId(1), "", "", "", 0, new Domain.GarmentPreparings.ValueObjects.UomId(1), "", "", 0, 50, guidPrepare,null).GetReadModel()
				}.AsQueryable());

			var rep = new Domain.GarmentPreparings.GarmentPreparing(guidPrepare, 1, "", new Domain.GarmentPreparings.ValueObjects.UnitDepartmentId(1), "", "", DateTimeOffset.Now, "roNo", "", true, new BuyerId(1), null, null);


		

			_mockGarmentPreparingRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentPreparingReadModel>
				{
					new Domain.GarmentPreparings.GarmentPreparing(guidPrepare,1,"",new Domain.GarmentPreparings.ValueObjects.UnitDepartmentId(1),"","",DateTimeOffset.Now,"roNo","",true,new BuyerId(1), null,null).GetReadModel()

		}.AsQueryable());

			//RemoveGarmentPreparingCommand RemoveGarmentPreparingCommand = new RemoveGarmentPreparingCommand();
			//RemoveGarmentPreparingCommand.SetId(guidPrepare);
			//_mockGarmentPreparingRepository
			//	.Setup(s => s.Find(It.IsAny<Expression<Func<GarmentPreparingReadModel, bool>>>()))
   //             .Returns(new List<GarmentPreparing>());

            var result = await unitUnderTest.Handle(getHendlerMonHisDelPreQuery, cancellationToken);


			result.Should().NotBeNull();
		}

		[Fact]
		public async Task Handle_MonHisDeleted_StateUnderTest_ExpectedBehavior_NULLdATE2()
		{

			GetMonPreHistoryDelQueryHandler unitUnderTest = CreateGetMonPreHistoryDelQueryHandler();
			CancellationToken cancellationToken = CancellationToken.None;

			Guid guidPrepare = Guid.NewGuid();
			Guid guidPrepareItem = Guid.NewGuid();

			//GetMonPreHistoryDelQuery getHendlerMonHisDelPreQuery = new GetMonPreHistoryDelQuery(null, DateTime.Now, DateTime.Now);
			GetMonPreHistoryDelQuery getHendlerMonHisDelPreQuery = new GetMonPreHistoryDelQuery(null, null, null);

			_mockGarmentPreparingItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentPreparingItemReadModel>
				{
					new Domain.GarmentPreparings.GarmentPreparingItem(guidPrepareItem, 1, new Domain.GarmentPreparings.ValueObjects.ProductId(1), "", "", "", 0, new Domain.GarmentPreparings.ValueObjects.UomId(1), "", "", 0, 50, guidPrepare,null).GetReadModel()
				}.AsQueryable());

			var rep = new Domain.GarmentPreparings.GarmentPreparing(guidPrepare, 1, "", new Domain.GarmentPreparings.ValueObjects.UnitDepartmentId(1), "", "", DateTimeOffset.Now, "roNo", "", true, new BuyerId(1), null, null);



			_mockGarmentPreparingRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentPreparingReadModel>
				{
					new Domain.GarmentPreparings.GarmentPreparing(guidPrepare,1,"",new Domain.GarmentPreparings.ValueObjects.UnitDepartmentId(1),"","",DateTimeOffset.Now,"roNo","",true,new BuyerId(1), null,null).GetReadModel()

		}.AsQueryable());

			// Test case 1: both dateFrom and dateTo are null
			var result1 = await unitUnderTest.Handle(new GetMonPreHistoryDelQuery(null, null, null), cancellationToken);
			result1.Should().NotBeNull();

			// Test case 2: dateFrom is null and dateTo is not null
			var result2 = await unitUnderTest.Handle(new GetMonPreHistoryDelQuery(null,null, DateTime.Now), cancellationToken);
			result2.Should().NotBeNull();

			// Test case 3: dateFrom is not null and dateTo is null
			var result3 = await unitUnderTest.Handle(new GetMonPreHistoryDelQuery(null, DateTime.Now, null), cancellationToken);
			result3.Should().NotBeNull();

			// Test case 4: both dateFrom and dateTo are invalid dates
			var result4 = await unitUnderTest.Handle(new GetMonPreHistoryDelQuery(null,DateTime.MinValue, DateTime.MaxValue), cancellationToken);
			result4.Should().NotBeNull();
		}
	}
}
