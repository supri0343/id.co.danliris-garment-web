using Barebone.Tests;
using FluentAssertions;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Application.GarmentFinishingOuts.Queries;
using Manufactures.Application.GarmentSewingOuts.Queries.MonitoringSewing;
using Manufactures.Domain.GarmentFinishingOuts;
using Manufactures.Domain.GarmentFinishingOuts.ReadModels;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
using Manufactures.Domain.GarmentSewingOuts;
using Manufactures.Domain.GarmentSewingOuts.ReadModels;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.CostCalculationGarmentDataProductionReport;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.HOrderDataProductionReport;

namespace Manufactures.Tests.Queries.GarmentFinishingOuts
{
	public class MonitoringFinishingCommandHandlerTest : BaseCommandUnitTest
	{
		private readonly Mock<IGarmentSewingOutRepository> _mockGarmentSewingOutRepository;
		private readonly Mock<IGarmentSewingOutItemRepository> _mockGarmentSewingOutItemRepository;
		private readonly Mock<IGarmentFinishingOutRepository> _mockGarmentFinishingOutRepository;
		private readonly Mock<IGarmentFinishingOutItemRepository> _mockGarmentFinishingOutItemRepository;
		protected readonly Mock<IHttpClientService> _mockhttpService;
		private Mock<IServiceProvider> serviceProviderMock;
		public MonitoringFinishingCommandHandlerTest()
		{
			_mockGarmentFinishingOutRepository = CreateMock<IGarmentFinishingOutRepository>();
			_mockGarmentFinishingOutItemRepository = CreateMock<IGarmentFinishingOutItemRepository>();

			_MockStorage.SetupStorage(_mockGarmentFinishingOutRepository);
			_MockStorage.SetupStorage(_mockGarmentFinishingOutItemRepository);

			_mockGarmentSewingOutRepository = CreateMock<IGarmentSewingOutRepository>();
			_mockGarmentSewingOutItemRepository = CreateMock<IGarmentSewingOutItemRepository>();

			_MockStorage.SetupStorage(_mockGarmentSewingOutRepository);
			_MockStorage.SetupStorage(_mockGarmentSewingOutItemRepository);

			serviceProviderMock = new Mock<IServiceProvider>();
			_mockhttpService = CreateMock<IHttpClientService>();

            List<CostCalViewModel> costCalViewModels = new List<CostCalViewModel> {
                new CostCalViewModel
                {
                    ro="ro",
                    comodityName="",
                    buyerCode="",
                    hours=10
                }
            };
		
			_mockhttpService.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<string>()))
				.ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{\"data\": " + JsonConvert.SerializeObject(costCalViewModels) + "}") });
			serviceProviderMock.Setup(x => x.GetService(typeof(IHttpClientService))).Returns(_mockhttpService.Object);
		}
		private GetMonitoringFinishingQueryHandler CreateGetMonitoringFinishingQueryHandler()
		{
			return new GetMonitoringFinishingQueryHandler(_MockStorage.Object, serviceProviderMock.Object);
		}
		[Fact]
		public async Task Handle_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			GetMonitoringFinishingQueryHandler unitUnderTest = CreateGetMonitoringFinishingQueryHandler();
			CancellationToken cancellationToken = CancellationToken.None;

			Guid guidFinishingOut = Guid.NewGuid();
			Guid guidFinishingOutItem = Guid.NewGuid();
			Guid guidSewingOut = Guid.NewGuid();
			Guid guidSewingOutItem = Guid.NewGuid();

			GetMonitoringFinishingQuery getMonitoring = new GetMonitoringFinishingQuery(1, 25, "{}", 1, DateTime.Now, DateTime.Now.AddDays(2), "token");

			_mockGarmentFinishingOutItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentFinishingOutItemReadModel>
				{
					new GarmentFinishingOutItem(guidFinishingOutItem,guidFinishingOut,new Guid(),new Guid(),new ProductId(1),"","","",new SizeId(1),"",10, new UomId(1),"","",10,10,10).GetReadModel()
				}.AsQueryable());

			_mockGarmentFinishingOutRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentFinishingOutReadModel>
				{
					new GarmentFinishingOut(guidFinishingOut,"",new UnitDepartmentId(1),"","","",DateTimeOffset.Now,"ro","",new UnitDepartmentId(1),"","",new GarmentComodityId(1),"","",false).GetReadModel()
				}.AsQueryable());

			_mockGarmentSewingOutItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSewingOutItemReadModel>
				{
					new GarmentSewingOutItem(guidSewingOutItem,guidSewingOut,new Guid(),new Guid(), new ProductId(1),"","","",new SizeId(1),"",0, new UomId(1),"","",10,100,100).GetReadModel()
				}.AsQueryable());

			_mockGarmentSewingOutRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSewingOutReadModel>
				{
					new GarmentSewingOut(guidSewingOut,"",new BuyerId(1),"","",new UnitDepartmentId(1),"","","",DateTimeOffset.Now,"ro","",new UnitDepartmentId(1),"","",new GarmentComodityId(1),"","",true).GetReadModel()
				}.AsQueryable());

			// Act
			var result = await unitUnderTest.Handle(getMonitoring, cancellationToken);

			// Assert
			result.Should().NotBeNull();
		}
	}
}
