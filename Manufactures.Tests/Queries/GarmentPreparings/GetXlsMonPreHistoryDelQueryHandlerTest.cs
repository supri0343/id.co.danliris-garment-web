using Barebone.Tests;
using FluentAssertions;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
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
using Manufactures.Domain.GarmentPreparings.ReadModels;
using Manufactures.Domain.GarmentPreparings.Repositories;
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
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.ExpenditureROResult;

namespace Manufactures.Tests.Queries.GarmentPreparings
{
    public class GetXlsMonPreHistoryDelQueryHandlerTest: BaseCommandUnitTest
    {
        private readonly Mock<IGarmentPreparingRepository> _mockGarmentPreparingRepository;
        private readonly Mock<IGarmentPreparingItemRepository> _mockGarmentPreparingItemRepository;

        protected readonly Mock<IHttpClientService> _mockhttpService;
        private Mock<IServiceProvider> serviceProviderMock;


        public GetXlsMonPreHistoryDelQueryHandlerTest()
        {
            _mockGarmentPreparingRepository = CreateMock<IGarmentPreparingRepository>();
            _mockGarmentPreparingItemRepository = CreateMock<IGarmentPreparingItemRepository>();
            _MockStorage.SetupStorage(_mockGarmentPreparingRepository);
            _MockStorage.SetupStorage(_mockGarmentPreparingItemRepository);
            serviceProviderMock = new Mock<IServiceProvider>();
            _mockhttpService = CreateMock<IHttpClientService>();
        }
        private GetXlsMonPreHistoryDelQueryHandler CreateGetXlsMonPreHistoryDelQueryHandler()
        {
            return new GetXlsMonPreHistoryDelQueryHandler(_MockStorage.Object, serviceProviderMock.Object);
        }

		//=====----->mdp unit test history deleted prepare XLS <----====//
		[Fact]
		public async Task Handle_StateUnderTest_ExpectedBehavior_hISTORIdEL()
		{
			// Arrange
			GetXlsMonPreHistoryDelQueryHandler unitUnderTest = CreateGetXlsMonPreHistoryDelQueryHandler();
			CancellationToken cancellationToken = CancellationToken.None;

			Guid guidPrepare = Guid.NewGuid();
			Guid guidPrepareItem = Guid.NewGuid();

			GetXlsMonPreHistoryDelQuery getXlsMonPreHistoryDelQuery = new GetXlsMonPreHistoryDelQuery(null, null, null);


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
		
			// Act
			var result = await unitUnderTest.Handle(getXlsMonPreHistoryDelQuery, cancellationToken);

			// Assert
			result.Should().NotBeNull();
		}


	}
}
