using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.GarmentServiceSampleSewings.CommandHandlers;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentPreparings.Repositories;
using Manufactures.Domain.GarmentSewingIns;
using Manufactures.Domain.GarmentSewingIns.ReadModels;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.ReadModels;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.Repositories;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSample.GarmentServiceSampleSewings
{
    public class PlaceGarmentServiceSampleSewingCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentServiceSampleSewingRepository> _mockServiceSampleSewingRepository;
        private readonly Mock<IGarmentServiceSampleSewingItemRepository> _mockServiceSampleSewingItemRepository;
        private readonly Mock<IGarmentServiceSampleSewingDetailRepository> _mockServiceSampleSewingDetailRepository;
        private readonly Mock<IGarmentSewingInRepository> _mockSewingInRepository;
        private readonly Mock<IGarmentSewingInItemRepository> _mockSewingInItemRepository;
        private readonly Mock<IGarmentPreparingRepository> _mockGarmentPreparingRepository;

        public PlaceGarmentServiceSampleSewingCommandHandlerTests()
        {
            _mockServiceSampleSewingRepository = CreateMock<IGarmentServiceSampleSewingRepository>();
            _mockServiceSampleSewingItemRepository = CreateMock<IGarmentServiceSampleSewingItemRepository>();
            _mockServiceSampleSewingDetailRepository = CreateMock<IGarmentServiceSampleSewingDetailRepository>();
            _mockSewingInRepository = CreateMock<IGarmentSewingInRepository>();
            _mockSewingInItemRepository = CreateMock<IGarmentSewingInItemRepository>();
            _mockGarmentPreparingRepository = CreateMock<IGarmentPreparingRepository>();

            _MockStorage.SetupStorage(_mockServiceSampleSewingRepository);
            _MockStorage.SetupStorage(_mockServiceSampleSewingItemRepository);
            _MockStorage.SetupStorage(_mockServiceSampleSewingDetailRepository);
            _MockStorage.SetupStorage(_mockSewingInRepository);
            _MockStorage.SetupStorage(_mockSewingInItemRepository);
            _MockStorage.SetupStorage(_mockGarmentPreparingRepository);
        }

        private PlaceGarmentServiceSampleSewingCommandHandler CreatePlaceGarmentServiceSampleSewingCommandHandler()
        {
            return new PlaceGarmentServiceSampleSewingCommandHandler(_MockStorage.Object);
        }

        /*[Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid sewingInItemGuid = Guid.NewGuid();
            Guid SewingInGuid = Guid.NewGuid();
            Guid sewingOutItemGuid = Guid.NewGuid();
            PlaceGarmentServiceSampleSewingCommandHandler unitUnderTest = CreatePlaceGarmentServiceSampleSewingCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentServiceSampleSewingCommand placeGarmentServiceSampleSewingCommand = new PlaceGarmentServiceSampleSewingCommand()
            {
                Buyer = new Buyer(1, "BuyerCode", "BuyerName"),
                Items = new List<GarmentServiceSampleSewingItemValueObject>
                {
                    new GarmentServiceSampleSewingItemValueObject
                    {
                        RONo = "RONo",
                        Article = "Article",
                        Comodity = new GarmentComodity(1, "ComoCode", "ComoName"),
                        Buyer = new Buyer(1, "BuyerCode", "BuyerName"),
                        Details= new List<GarmentServiceSampleSewingDetailValueObject>
                        {
                            new GarmentServiceSampleSewingDetailValueObject
                            {
                                Product = new Product(1, "ProductCode", "ProductName"),
                                Uom = new Uom(1, "UomUnit"),
                                SewingInId= new Guid(),
                                SewingInItemId=sewingInItemGuid,
                                IsSave=true,
                                Quantity=1,
                                DesignColor= "ColorD",
                                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                            }
                        }
                        
                    }
                },

            };
            GarmentSewingIn garmentSewingIn = new GarmentSewingIn(
                SewingInGuid, null, "SEWING", Guid.Empty, null, new UnitDepartmentId(1), null, null,
                new UnitDepartmentId(1), null, null, "RONo", null, new GarmentComodityId(1), null, null, DateTimeOffset.Now);
            GarmentSewingInItem garmentSewingInItem = new GarmentSewingInItem(sewingInItemGuid, SewingInGuid, sewingOutItemGuid, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, new ProductId(1), null, null, "ColorD", new SizeId(1), null, 10, new UomId(1), null, null, 0, 1, 1);
            GarmentServiceSampleSewingDetail garmentServiceSampleSewingDetail = new GarmentServiceSampleSewingDetail(new Guid(), new Guid(), SewingInGuid, sewingInItemGuid, new ProductId(1), null, null, "ColorD", 1, new UomId(1), null, new UnitDepartmentId(1), null, null);
            _mockSewingInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSewingInReadModel>()
                {
                    garmentSewingIn.GetReadModel()
                }.AsQueryable());
            _mockSewingInItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSewingInItemReadModel>()
                {
                    garmentSewingInItem.GetReadModel()
                }.AsQueryable());

            _mockServiceSampleSewingRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSampleSewingReadModel>().AsQueryable());
            //_mockServiceSampleSewingItemRepository
            //    .Setup(s => s.Query)
            //    .Returns(new List<GarmentServiceSampleSewingItemReadModel>().AsQueryable());
            _mockServiceSampleSewingDetailRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSampleSewingDetailReadModel>() {
                    garmentServiceSampleSewingDetail.GetReadModel()
                }.AsQueryable());


            _mockServiceSampleSewingRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSampleSewing>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSampleSewing>()));
            _mockServiceSampleSewingItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSampleSewingItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSampleSewingItem>()));
            _mockServiceSampleSewingDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSampleSewingDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSampleSewingDetail>()));

            _mockGarmentPreparingRepository
                .Setup(s => s.RoChecking(It.IsAny<IEnumerable<string>>(), string.Empty))
                .Returns(true);

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(placeGarmentServiceSampleSewingCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }*/
    }
}
