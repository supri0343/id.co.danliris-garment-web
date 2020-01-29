using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentAdjustments.CommandHandlers;
using Manufactures.Domain.GarmentAdjustments;
using Manufactures.Domain.GarmentAdjustments.Commands;
using Manufactures.Domain.GarmentAdjustments.ReadModels;
using Manufactures.Domain.GarmentAdjustments.Repositories;
using Manufactures.Domain.GarmentAdjustments.ValueObjects;
using Manufactures.Domain.GarmentSewingDOs;
using Manufactures.Domain.GarmentSewingDOs.ReadModels;
using Manufactures.Domain.GarmentSewingDOs.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentAdjustments
{
    public class PlaceGarmentAdjustmentCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentAdjustmentRepository> _mockAdjustmentRepository;
        private readonly Mock<IGarmentAdjustmentItemRepository> _mockAdjustmentItemRepository;
        private readonly Mock<IGarmentSewingDOItemRepository> _mockSewingDOItemRepository;

        public PlaceGarmentAdjustmentCommandHandlerTests()
        {
            _mockAdjustmentRepository = CreateMock<IGarmentAdjustmentRepository>();
            _mockAdjustmentItemRepository = CreateMock<IGarmentAdjustmentItemRepository>();
            _mockSewingDOItemRepository = CreateMock<IGarmentSewingDOItemRepository>();

            _MockStorage.SetupStorage(_mockAdjustmentRepository);
            _MockStorage.SetupStorage(_mockAdjustmentItemRepository);
            _MockStorage.SetupStorage(_mockSewingDOItemRepository);
        }

        private PlaceGarmentAdjustmentCommandHandler CreatePlaceGarmentAdjustmentCommandHandler()
        {
            return new PlaceGarmentAdjustmentCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid sewingDOItemGuid = Guid.NewGuid();
            Guid sewingDOGuid = Guid.NewGuid();
            PlaceGarmentAdjustmentCommandHandler unitUnderTest = CreatePlaceGarmentAdjustmentCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentAdjustmentCommand placeGarmentAdjustmentCommand = new PlaceGarmentAdjustmentCommand()
            {
                RONo = "RONo",
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                AdjustmentType="LOADING",
                AdjustmentDate = DateTimeOffset.Now,
                Article = "Article",
                Comodity = new GarmentComodity(1, "ComoCode", "ComoName"),
                Items = new List<GarmentAdjustmentItemValueObject>
                {
                    new GarmentAdjustmentItemValueObject
                    {
                        IsSave=true,
                        SewingDOItemId=sewingDOItemGuid,
                        Size=new SizeValueObject(1, "Size"),
                        Quantity=1,
                        RemainingQuantity=2,
                        Product= new Product(1, "ProdCode", "ProdName"),
                        Uom=new Uom(1, "Uom"),
                    }
                },

            };

            _mockAdjustmentRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentAdjustmentReadModel>().AsQueryable());
            _mockSewingDOItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSewingDOItemReadModel>
                {
                    new GarmentSewingDOItemReadModel(sewingDOItemGuid)
                }.AsQueryable());

            _mockAdjustmentRepository
                .Setup(s => s.Update(It.IsAny<GarmentAdjustment>()))
                .Returns(Task.FromResult(It.IsAny<GarmentAdjustment>()));
            _mockAdjustmentItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentAdjustmentItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentAdjustmentItem>()));
            _mockSewingDOItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSewingDOItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSewingDOItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(placeGarmentAdjustmentCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}