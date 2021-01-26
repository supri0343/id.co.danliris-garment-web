using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSubcon.GarmentServiceSubconCuttings.CommandHandlers;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSubcon.GarmentServiceSubconCuttings
{
    public class PlaceGarmentServiceSubconCuttingCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentServiceSubconCuttingRepository> _mockServiceSubconCuttingRepository;
        private readonly Mock<IGarmentServiceSubconCuttingItemRepository> _mockServiceSubconCuttingItemRepository;

        public PlaceGarmentServiceSubconCuttingCommandHandlerTests()
        {
            _mockServiceSubconCuttingRepository = CreateMock<IGarmentServiceSubconCuttingRepository>();
            _mockServiceSubconCuttingItemRepository = CreateMock<IGarmentServiceSubconCuttingItemRepository>();

            _MockStorage.SetupStorage(_mockServiceSubconCuttingRepository);
            _MockStorage.SetupStorage(_mockServiceSubconCuttingItemRepository);
        }
        private PlaceGarmentServiceSubconCuttingCommandHandler CreatePlaceGarmentServiceSubconCuttingCommandHandler()
        {
            return new PlaceGarmentServiceSubconCuttingCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_BORDIR()
        {
            // Arrange
            Guid sewingInItemGuid = Guid.NewGuid();
            PlaceGarmentServiceSubconCuttingCommandHandler unitUnderTest = CreatePlaceGarmentServiceSubconCuttingCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentServiceSubconCuttingCommand placeGarmentServiceSubconCuttingCommand = new PlaceGarmentServiceSubconCuttingCommand()
            {
                RONo = "RONo",
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                Article = "Article",
                Comodity = new GarmentComodity(1, "ComoCode", "ComoName"),
                SubconDate = DateTimeOffset.Now,
                SubconType="BORDIR",
                Items = new List<GarmentServiceSubconCuttingItemValueObject>
                {
                    new GarmentServiceSubconCuttingItemValueObject
                    {
                        Product = new Product(1, "ProductCode", "ProductName"),
                        IsSave=true,
                        Quantity=1,
                        DesignColor= "ColorD",
                        CuttingInQuantity=1,
                        CuttingInDetailId=Guid.NewGuid(),
                        ServiceSubconCuttingId=Guid.NewGuid()
                    }
                },

            };

            _mockServiceSubconCuttingRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconCuttingReadModel>().AsQueryable());
            _mockServiceSubconCuttingRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconCutting>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconCutting>()));
            _mockServiceSubconCuttingItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconCuttingItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconCuttingItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(placeGarmentServiceSubconCuttingCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_PRINT()
        {
            // Arrange
            Guid sewingInItemGuid = Guid.NewGuid();
            PlaceGarmentServiceSubconCuttingCommandHandler unitUnderTest = CreatePlaceGarmentServiceSubconCuttingCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentServiceSubconCuttingCommand placeGarmentServiceSubconCuttingCommand = new PlaceGarmentServiceSubconCuttingCommand()
            {
                RONo = "RONo",
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                Article = "Article",
                Comodity = new GarmentComodity(1, "ComoCode", "ComoName"),
                SubconDate = DateTimeOffset.Now,
                SubconType = "PRINT",
                Items = new List<GarmentServiceSubconCuttingItemValueObject>
                {
                    new GarmentServiceSubconCuttingItemValueObject
                    {
                        Product = new Product(1, "ProductCode", "ProductName"),
                        IsSave=true,
                        Quantity=1,
                        DesignColor= "ColorD",
                        CuttingInQuantity=1,
                        CuttingInDetailId=Guid.NewGuid(),
                        ServiceSubconCuttingId=Guid.NewGuid()
                    }
                },

            };

            _mockServiceSubconCuttingRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconCuttingReadModel>().AsQueryable());
            _mockServiceSubconCuttingRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconCutting>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconCutting>()));
            _mockServiceSubconCuttingItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconCuttingItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconCuttingItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(placeGarmentServiceSubconCuttingCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_PLISKET()
        {
            // Arrange
            Guid sewingInItemGuid = Guid.NewGuid();
            PlaceGarmentServiceSubconCuttingCommandHandler unitUnderTest = CreatePlaceGarmentServiceSubconCuttingCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentServiceSubconCuttingCommand placeGarmentServiceSubconCuttingCommand = new PlaceGarmentServiceSubconCuttingCommand()
            {
                RONo = "RONo",
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                Article = "Article",
                Comodity = new GarmentComodity(1, "ComoCode", "ComoName"),
                SubconDate = DateTimeOffset.Now,
                SubconType = "PLISKET",
                Items = new List<GarmentServiceSubconCuttingItemValueObject>
                {
                    new GarmentServiceSubconCuttingItemValueObject
                    {
                        Product = new Product(1, "ProductCode", "ProductName"),
                        IsSave=true,
                        Quantity=1,
                        DesignColor= "ColorD",
                        CuttingInQuantity=1,
                        CuttingInDetailId=Guid.NewGuid(),
                        ServiceSubconCuttingId=Guid.NewGuid()
                    }
                },

            };

            _mockServiceSubconCuttingRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconCuttingReadModel>().AsQueryable());
            _mockServiceSubconCuttingRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconCutting>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconCutting>()));
            _mockServiceSubconCuttingItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconCuttingItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconCuttingItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(placeGarmentServiceSubconCuttingCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
