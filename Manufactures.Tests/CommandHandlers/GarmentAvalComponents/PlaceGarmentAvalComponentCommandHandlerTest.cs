using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentAvalComponents.CommandHandlers;
using Manufactures.Domain.GarmentAvalComponents;
using Manufactures.Domain.GarmentAvalComponents.Commands;
using Manufactures.Domain.GarmentAvalComponents.Repositories;
using Manufactures.Domain.GarmentAvalComponents.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentAvalComponents
{
    public class PlaceGarmentAvalComponentCommandHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentAvalComponentRepository> _mockGarmentAvalComponentRepository;
        private readonly Mock<IGarmentAvalComponentItemRepository> _mockGarmentAvalComponentItemRepository;

        public PlaceGarmentAvalComponentCommandHandlerTest()
        {
            _mockGarmentAvalComponentRepository = CreateMock<IGarmentAvalComponentRepository>();
            _mockGarmentAvalComponentItemRepository = CreateMock<IGarmentAvalComponentItemRepository>();

            _MockStorage.SetupStorage(_mockGarmentAvalComponentRepository);
            _MockStorage.SetupStorage(_mockGarmentAvalComponentItemRepository);
        }

        private PlaceGarmentAvalComponentCommandHandler CreatePlaceGarmentAvalComponentCommandHandler()
        {
            return new PlaceGarmentAvalComponentCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            PlaceGarmentAvalComponentCommandHandler unitUnderTest = CreatePlaceGarmentAvalComponentCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;

            Guid guid = Guid.NewGuid();

            PlaceGarmentAvalComponentCommand placeGarmentAvalComponentCommand = new PlaceGarmentAvalComponentCommand()
            {
                Unit = new UnitDepartment(),
                Comodity = new GarmentComodity(),
                Items = new List<PlaceGarmentAvalComponentItemValueObject>
                {
                    new PlaceGarmentAvalComponentItemValueObject
                    {
                        Product = new Product(),
                        Size = new SizeValueObject()
                    }
                }
            };

            _mockGarmentAvalComponentRepository
                .Setup(s => s.Update(It.IsAny<GarmentAvalComponent>()))
                .Returns(Task.FromResult(It.IsAny<GarmentAvalComponent>()));

            _mockGarmentAvalComponentItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentAvalComponentItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentAvalComponentItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(placeGarmentAvalComponentCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
