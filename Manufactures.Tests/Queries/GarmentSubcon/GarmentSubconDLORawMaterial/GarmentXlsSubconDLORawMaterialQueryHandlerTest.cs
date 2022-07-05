using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;
using System.Linq;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.ReadModels;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes;
using FluentAssertions;
using Xunit;
using Barebone.Tests;
using Moq;
using Manufactures.Application.GarmentSubcon.Queries.GarmentSubconDLORawMaterialReport;
using System.Threading.Tasks;
using System.Threading;

namespace Manufactures.Tests.Queries.GarmentSubcon.GarmentSubconDLORawMaterial
{
    public class GarmentXlsSubconDLORawMaterialQueryHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSubconDeliveryLetterOutRepository> _mockgarmentSubconDeliveryLetterOutRepository;
        private readonly Mock<IGarmentServiceSubconShrinkagePanelRepository> _mockgarmentSubconShrinkagePanelRepository;
        private readonly Mock<IGarmentServiceSubconFabricWashRepository> _mockgarmentSubconFabricWashRepoasitory;

        private Mock<IServiceProvider> serviceProviderMock;

        public GarmentXlsSubconDLORawMaterialQueryHandlerTest()
        {

            _mockgarmentSubconDeliveryLetterOutRepository = CreateMock<IGarmentSubconDeliveryLetterOutRepository>();
            _mockgarmentSubconShrinkagePanelRepository = CreateMock<IGarmentServiceSubconShrinkagePanelRepository>();
            _mockgarmentSubconFabricWashRepoasitory = CreateMock<IGarmentServiceSubconFabricWashRepository>();

            _MockStorage.SetupStorage(_mockgarmentSubconDeliveryLetterOutRepository);
            _MockStorage.SetupStorage(_mockgarmentSubconShrinkagePanelRepository);
            _MockStorage.SetupStorage(_mockgarmentSubconFabricWashRepoasitory);

            serviceProviderMock = new Mock<IServiceProvider>();

        }

        private GetXlsGarmentSubconDLORawMaterialReportQueryHandler CreateGetPrepareTraceableQueryHandler()
        {
            return new GetXlsGarmentSubconDLORawMaterialReportQueryHandler(_MockStorage.Object, serviceProviderMock.Object);
        }

        //[Fact]
        //public async Task Handle_StateUnderTest_ExpectedBehavior()
        //{
        //    GetXlsGarmentRealizationSubconReportQueryHandler unitUnderTest = CreateGetPrepareTraceableQueryHandler();
        //    CancellationToken cancellationToken = CancellationToken.None;


        //    Guid guidSubconCustomsIn = Guid.NewGuid();
        //    Guid guidSubconCustomsInItem = Guid.NewGuid();
        //    Guid guidSubconCustomsInItem2 = Guid.NewGuid();
        //    Guid guidSubconCustomsOut = Guid.NewGuid();
        //    Guid guidSubconCustomsOutItem = Guid.NewGuid();
        //    Guid guidSubconCustomsOutItem2 = Guid.NewGuid();
        //    Guid guidSubconContract = Guid.NewGuid();
        //    Guid guidSubconContract2 = Guid.NewGuid();
        //    Guid guidSubconContractItem = Guid.NewGuid();
        //    Guid guidSubconContractItem2 = Guid.NewGuid();


        //    GetXlsGarmentRealizationSubconReportQuery getMonitoring = new GetXlsGarmentRealizationSubconReportQuery(1, 25, "", "subconcontract", "token");
        //    GetXlsGarmentRealizationSubconReportQuery getMonitoring2 = new GetXlsGarmentRealizationSubconReportQuery(1, 25, "", "subconcontract2", "token");


        //    _mockgarmentSubconContractRepository
        //        .Setup(s => s.Query)
        //        .Returns(new List<GarmentSubconContractReadModel>
        //        {
        //                new GarmentSubconContract(guidSubconContract, "contractType", "subconcontract", "agreementNo", new SupplierId (1), "supplierCode", "supplierName", "jobType", "bPJNo", "finishedGoodType", 12, DateTimeOffset.Now, DateTimeOffset.Now, true, new BuyerId(1), "buyerCode", "buyerName", "subconCategory", new UomId(1), "uomUnit", "sKEPNo", DateTimeOffset.Now).GetReadModel(),
        //                new GarmentSubconContract(guidSubconContract2, "contractType", "subconcontract2", "agreementNo", new SupplierId (1), "supplierCode", "supplierName", "jobType", "bPJNo", "finishedGoodType", 12, DateTimeOffset.Now, DateTimeOffset.Now, true, new BuyerId(1), "buyerCode", "buyerName", "subconCategory2", new UomId(1), "uomUnit", "sKEPNo", DateTimeOffset.Now).GetReadModel(),

        //        }.AsQueryable());

        //    _mockgarmentSubconContractItemRepository
        //         .Setup(s => s.Query)
        //        .Returns(new List<GarmentSubconContractItemReadModel>
        //        {
        //            new GarmentSubconContractItem(guidSubconContractItem, guidSubconContract,new ProductId(1), "productCode", "productName", 21,new UomId(1), "uomUnit").GetReadModel(),
        //            new GarmentSubconContractItem(guidSubconContractItem2, guidSubconContract2,new ProductId(1), "productCode", "productName", 21,new UomId(1), "uomUnit").GetReadModel()
        //        }.AsQueryable());

        //    _mockgarmentSubconCustomsInRepository
        //         .Setup(s => s.Query)
        //        .Returns(new List<GarmentSubconCustomsInReadModel>
        //        {
        //            new GarmentSubconCustomsIn(guidSubconCustomsIn, "bcNo", DateTimeOffset.Now, "bcType", "subconType", guidSubconContract, "subconcontract", new SupplierId(1), "supplierCode", "supplierName", "remark", true, "subconCategory").GetReadModel()

        //        }.AsQueryable());

        //    _mockgarmentSubconCustomsInItemRepository
        //         .Setup(s => s.Query)
        //        .Returns(new List<GarmentSubconCustomsInItemReadModel>
        //        {
        //            new GarmentSubconCustomsInItem(guidSubconCustomsInItem, guidSubconCustomsIn, new SupplierId(1), "supplierCode", "supplierName", 1, "doNo", 2).GetReadModel(),
        //            new GarmentSubconCustomsInItem(guidSubconCustomsInItem2, guidSubconCustomsIn, new SupplierId(1), "supplierCode", "supplierName", 1, "doNo", 2).GetReadModel()
        //        }.AsQueryable());

        //    _mockgarmentSubconCustomsOutRepository
        //         .Setup(s => s.Query)
        //        .Returns(new List<GarmentSubconCustomsOutReadModel>
        //        {
        //            new GarmentSubconCustomsOut(guidSubconCustomsOut, "customsOutNo", DateTimeOffset.Now, "customsOutType", "subconType", guidSubconContract2, "subconcontract2", new SupplierId(1), "supplierCode", "supplierName", "remark", "subconCategory").GetReadModel()
        //        }.AsQueryable());

        //    _mockgarmentSubconCustomsOutItemRepository
        //         .Setup(s => s.Query)
        //        .Returns(new List<GarmentSubconCustomsOutItemReadModel>
        //        {
        //            new GarmentSubconCustomsOutItem(guidSubconCustomsOutItem, guidSubconCustomsOut, "subconDLOutNo", Guid.NewGuid(), 2).GetReadModel(),
        //            new GarmentSubconCustomsOutItem(guidSubconCustomsOutItem2, guidSubconCustomsOut, "subconDLOutNo", Guid.NewGuid(), 2).GetReadModel()
        //        }.AsQueryable());


        //    var result = await unitUnderTest.Handle(getMonitoring, cancellationToken);
        //    var result2 = await unitUnderTest.Handle(getMonitoring2, cancellationToken);

        //    // Assert
        //    result.Should().NotBeNull();
        //    result2.Should().NotBeNull();

        //}
    }
}
