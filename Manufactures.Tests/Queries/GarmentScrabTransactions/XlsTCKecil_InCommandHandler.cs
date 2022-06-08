
using Barebone.Tests;
using FluentAssertions;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Application.GarmentScrapTransactions.Queries.GetMutationScrap;
using Manufactures.Application.GarmentScrapTransactions.Queries.GetMutationScrap.TCKecil;
using Manufactures.Domain.GarmentScrapSources;
using Manufactures.Domain.GarmentScrapSources.ReadModels;
using Manufactures.Domain.GarmentScrapSources.Repositories;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentScrabTransactions
{
    public class XlsTCKecil_InCommandHandler : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentScrapTransactionRepository> _mockGarmentScrapTransactionRepository;
        private readonly Mock<IGarmentScrapTransactionItemRepository> _mockGarmentScrapTransactionItemRepository;
        //protected readonly Mock<IHttpClientService> _mockhttpService;
        //private Mock<IServiceProvider> serviceProviderMock;

        public XlsTCKecil_InCommandHandler()
        {
            _mockGarmentScrapTransactionRepository = CreateMock<IGarmentScrapTransactionRepository>();
            _mockGarmentScrapTransactionItemRepository = CreateMock<IGarmentScrapTransactionItemRepository>();
            _MockStorage.SetupStorage(_mockGarmentScrapTransactionRepository);
            _MockStorage.SetupStorage(_mockGarmentScrapTransactionItemRepository);

            //serviceProviderMock = new Mock<IServiceProvider>();
            //_mockhttpService = CreateMock<IHttpClientService>();

            //List<ScrapListViewModel> scrapListViewModels = new List<ScrapListViewModel>
            //{
            //    new ScrapListViewModel
            //    {
            //        count = 1,
            //        scrapDtos = new List<ScrapDto>
            //        {
            //            new ScrapDto()
            //            {
            //                TransactionNo = "no1",
            //                TransactionDate = DateTimeOffset.Now,
            //                ScrapSourceName = "test",
            //                Quantity = 1,
            //                UomUnit = "Uom",
            //            },
            //            new ScrapDto()
            //            {
            //                TransactionNo = "no2",
            //                TransactionDate = DateTimeOffset.Now,
            //                ScrapSourceName = "test",
            //                Quantity = 2,
            //                UomUnit = "Uom",
            //            },
            //            new ScrapDto()
            //            {
            //                TransactionNo = "no3",
            //                TransactionDate = DateTimeOffset.Now,
            //                ScrapSourceName = "test",
            //                Quantity = 3,
            //                UomUnit = "Uom",
            //            },
            //        }
            //    }
            //};

            //_mockhttpService.Setup(x => x.SendAsync(It.IsAny<HttpMethod>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<HttpContent>()))
            //    .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{\"data\": " + JsonConvert.SerializeObject(scrapListViewModels) + "}") });
            //serviceProviderMock.Setup(x => x.GetService(typeof(IHttpClientService))).Returns(_mockhttpService.Object);
        }

        private GetXlsTCKecil_in_QueryHandler CreateGetXlsTCKecil_in_QueryHandler()
        {
            return new GetXlsTCKecil_in_QueryHandler(_MockStorage.Object);
        }

        //[Fact]
        //public async Task Handle_StateUnderTest_ExpectedBehavior()
        //{
        //    // Arrage
        //    GetXlsTCKecil_in_QueryHandler unitUnderTest = CreateGetXlsTCKecil_in_QueryHandler();
        //    CancellationToken cancellationToken = CancellationToken.None;

        //    Guid guidScrapTransaction = Guid.NewGuid();
        //    Guid guidScrapTransactionItem = Guid.NewGuid();
        //    Guid guidScrapClassification = Guid.NewGuid();
        //    Guid guidScrapSource = Guid.NewGuid();
        //    Guid guidScrapDest = Guid.NewGuid();

        //    _mockGarmentScrapTransactionItemRepository
        //        .Setup(s => s.Query)
        //        .Returns(new List<GarmentScrapTransactionItemReadModel>
        //        {
        //            new GarmentScrapTransactionItem(guidScrapTransactionItem, guidScrapTransaction, guidScrapClassification, "class01", 20, 1, "", "").GetReadModel()
        //        }.AsQueryable());

        //    _mockGarmentScrapTransactionRepository
        //        .Setup(s => s.Query)
        //        .Returns(new List<GarmentScrapTransactionReadModel>
        //        {
        //            new GarmentScrapTransaction(guidScrapTransaction, "", "IN", DateTimeOffset.Now, guidScrapSource, "", guidScrapDest, "" ).GetReadModel()
        //        }.AsQueryable());

        //    GetXlsTCKecil_in_Query xlstckecil_in = new GetXlsTCKecil_in_Query(DateTime.UtcNow, DateTime.UtcNow, "token");

        //    // Act
        //    var result = await unitUnderTest.Handle(xlstckecil_in, cancellationToken);

        //    // Assert
        //    result.Should().NotBeNull();
        //}
    }
}
