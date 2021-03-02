using Barebone.Tests;
using Manufactures.Application.GarmentSubcon.GarmentSubconDeliveryLetterOuts.CommandHandlers;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Commands;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Manufactures.Domain.Shared.ValueObjects;
using Xunit;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.ReadModels;
using System.Linq;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts;
using FluentAssertions;

namespace Manufactures.Tests.CommandHandlers.GarmentSubcon.GarmentSubconDeliveryLetterOuts
{
    public class PlaceGarmentSubconDeliveryLetterOutCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSubconDeliveryLetterOutRepository> _mockSubconDeliveryLetterOutRepository;
        private readonly Mock<IGarmentSubconDeliveryLetterOutItemRepository> _mockSubconDeliveryLetterOutItemRepository;

        public PlaceGarmentSubconDeliveryLetterOutCommandHandlerTests()
        {
            _mockSubconDeliveryLetterOutRepository = CreateMock<IGarmentSubconDeliveryLetterOutRepository>();
            _mockSubconDeliveryLetterOutItemRepository = CreateMock<IGarmentSubconDeliveryLetterOutItemRepository>();

            _MockStorage.SetupStorage(_mockSubconDeliveryLetterOutRepository);
            _MockStorage.SetupStorage(_mockSubconDeliveryLetterOutItemRepository);
        }
        private PlaceGarmentSubconDeliveryLetterOutCommandHandler CreatePlaceGarmentSubconDeliveryLetterOutCommandHandler()
        {
            return new PlaceGarmentSubconDeliveryLetterOutCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_BB()
        {
            // Arrange
            Guid sewingInItemGuid = Guid.NewGuid();
            PlaceGarmentSubconDeliveryLetterOutCommandHandler unitUnderTest = CreatePlaceGarmentSubconDeliveryLetterOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentSubconDeliveryLetterOutCommand placeGarmentSubconDeliveryLetterOutCommand = new PlaceGarmentSubconDeliveryLetterOutCommand()
            {
                ContractNo="test",
                ContractType="SUBCON BAHAN BAKU",
                DLDate=DateTimeOffset.Now,
                DLType="PROSES",
                EPOItemId=1,
                IsUsed=false,
                PONo="test",
                Remark="test",
                TotalQty=10,
                UsedQty=10,
                SubconContractId=new Guid(),
                UENId=1,
                UENNo="test",
                Items = new List<GarmentSubconDeliveryLetterOutItemValueObject>
                {
                    new GarmentSubconDeliveryLetterOutItemValueObject
                    {
                        Product = new Product(1, "ProductCode", "ProductName"),
                        Quantity=1,
                        DesignColor= "ColorD",
                        SubconDeliveryLetterOutId=Guid.NewGuid(),
                        FabricType="test",
                        ProductRemark="test",
                        UENItemId=1,
                        Uom=new Uom(1,"UomUnit"),
                        UomOut=new Uom(1,"UomUnit"),
                        ContractQuantity=1
                    }
                },

            };

            _mockSubconDeliveryLetterOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSubconDeliveryLetterOutReadModel>().AsQueryable());
            _mockSubconDeliveryLetterOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconDeliveryLetterOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconDeliveryLetterOut>()));
            _mockSubconDeliveryLetterOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconDeliveryLetterOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconDeliveryLetterOutItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(placeGarmentSubconDeliveryLetterOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_CT()
        {
            // Arrange
            Guid sewingInItemGuid = Guid.NewGuid();
            PlaceGarmentSubconDeliveryLetterOutCommandHandler unitUnderTest = CreatePlaceGarmentSubconDeliveryLetterOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentSubconDeliveryLetterOutCommand placeGarmentSubconDeliveryLetterOutCommand = new PlaceGarmentSubconDeliveryLetterOutCommand()
            {
                ContractNo = "test",
                ContractType = "SUBCON CUTTING",
                DLDate = DateTimeOffset.Now,
                DLType = "RE PROSES",
                EPOItemId = 1,
                IsUsed = false,
                PONo = "test",
                Remark = "test",
                TotalQty = 10,
                UsedQty = 10,
                SubconContractId = new Guid(),
                UENId = 1,
                UENNo = "test",
                Items = new List<GarmentSubconDeliveryLetterOutItemValueObject>
                {
                    new GarmentSubconDeliveryLetterOutItemValueObject
                    {
                        Product = new Product(1, "ProductCode", "ProductName"),
                        Quantity=1,
                        DesignColor= "ColorD",
                        SubconDeliveryLetterOutId=Guid.NewGuid(),
                        FabricType="test",
                        ProductRemark="test",
                        UENItemId=1,
                        Uom=new Uom(1,"UomUnit"),
                        UomOut=new Uom(1,"UomUnit"),
                        ContractQuantity=1
                    }
                },

            };

            _mockSubconDeliveryLetterOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSubconDeliveryLetterOutReadModel>().AsQueryable());
            _mockSubconDeliveryLetterOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconDeliveryLetterOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconDeliveryLetterOut>()));
            _mockSubconDeliveryLetterOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconDeliveryLetterOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconDeliveryLetterOutItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(placeGarmentSubconDeliveryLetterOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_JS()
        {
            // Arrange
            Guid sewingInItemGuid = Guid.NewGuid();
            PlaceGarmentSubconDeliveryLetterOutCommandHandler unitUnderTest = CreatePlaceGarmentSubconDeliveryLetterOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentSubconDeliveryLetterOutCommand placeGarmentSubconDeliveryLetterOutCommand = new PlaceGarmentSubconDeliveryLetterOutCommand()
            {
                ContractNo = "test",
                ContractType = "SUBCON JASA",
                DLDate = DateTimeOffset.Now,
                DLType = "RE PROSES",
                EPOItemId = 1,
                IsUsed = false,
                PONo = "test",
                Remark = "test",
                TotalQty = 10,
                UsedQty = 10,
                SubconContractId = new Guid(),
                UENId = 1,
                UENNo = "test",
                Items = new List<GarmentSubconDeliveryLetterOutItemValueObject>
                {
                    new GarmentSubconDeliveryLetterOutItemValueObject
                    {
                        Product = new Product(1, "ProductCode", "ProductName"),
                        Quantity=1,
                        DesignColor= "ColorD",
                        SubconDeliveryLetterOutId=Guid.NewGuid(),
                        FabricType="test",
                        ProductRemark="test",
                        UENItemId=1,
                        Uom=new Uom(1,"UomUnit"),
                        UomOut=new Uom(1,"UomUnit"),
                        ContractQuantity=1
                    }
                },

            };

            _mockSubconDeliveryLetterOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSubconDeliveryLetterOutReadModel>().AsQueryable());
            _mockSubconDeliveryLetterOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconDeliveryLetterOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconDeliveryLetterOut>()));
            _mockSubconDeliveryLetterOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconDeliveryLetterOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconDeliveryLetterOutItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(placeGarmentSubconDeliveryLetterOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
