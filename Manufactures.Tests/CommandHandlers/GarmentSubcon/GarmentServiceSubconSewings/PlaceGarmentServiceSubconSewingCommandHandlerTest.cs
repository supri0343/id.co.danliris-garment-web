using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSubcon.GarmentServiceSubconSewings.CommandHandlers;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSubcon.GarmentServiceSubconSewings
{
    public class PlaceGarmentServiceSubconSewingCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentServiceSubconSewingRepository> _mockServiceSubconSewingRepository;
        private readonly Mock<IGarmentServiceSubconSewingItemRepository> _mockServiceSubconSewingItemRepository;

        public PlaceGarmentServiceSubconSewingCommandHandlerTests()
        {
            _mockServiceSubconSewingRepository = CreateMock<IGarmentServiceSubconSewingRepository>();
            _mockServiceSubconSewingItemRepository = CreateMock<IGarmentServiceSubconSewingItemRepository>();

            _MockStorage.SetupStorage(_mockServiceSubconSewingRepository);
            _MockStorage.SetupStorage(_mockServiceSubconSewingItemRepository);
        }

        private PlaceGarmentServiceSubconSewingCommandHandler CreatePlaceGarmentServiceSubconSewingCommandHandler()
        {
            return new PlaceGarmentServiceSubconSewingCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid sewingInItemGuid = Guid.NewGuid();
            PlaceGarmentServiceSubconSewingCommandHandler unitUnderTest = CreatePlaceGarmentServiceSubconSewingCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentServiceSubconSewingCommand placeGarmentServiceSubconSewingCommand = new PlaceGarmentServiceSubconSewingCommand()
            {
                RONo = "RONo",
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                Article = "Article",
                IsDifferentSize = true,
                Buyer = new Buyer(1, "BuyerCode", "BuyerName"),
                Comodity = new GarmentComodity(1, "ComoCode", "ComoName"),
                Items = new List<GarmentServiceSubconSewingItemValueObject>
                {
                    new GarmentServiceSubconSewingItemValueObject
                    {
                        Product = new Product(1, "ProductCode", "ProductName"),
                        Uom = new Uom(1, "UomUnit"),
                        SewingInId= new Guid(),
                        SewingInItemId=sewingInItemGuid,
                        Color="Color",
                        Size=new SizeValueObject(1, "Size"),
                        IsSave=true,
                        Quantity=1,
                        DesignColor= "ColorD",
                    }
                },

            };

            _mockServiceSubconSewingRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconSewingReadModel>().AsQueryable());


            _mockServiceSubconSewingRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconSewing>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconSewing>()));
            _mockServiceSubconSewingItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconSewingItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconSewingItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(placeGarmentServiceSubconSewingCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
