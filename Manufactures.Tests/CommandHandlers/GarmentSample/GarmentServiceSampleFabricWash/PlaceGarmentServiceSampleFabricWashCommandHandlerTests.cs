using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.GarmentServiceSampleFabricWashes.CommandHandlers;
using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes;
using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes.ReadModels;
using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes.Repositories;
using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSample.GarmentServiceSampleFabricWashes
{
    public class PlaceGarmentServiceSampleFabricWashCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentServiceSampleFabricWashRepository> _mockServiceSampleFabricWashRepository;
        private readonly Mock<IGarmentServiceSampleFabricWashItemRepository> _mockServiceSampleFabricWashItemRepository;
        private readonly Mock<IGarmentServiceSampleFabricWashDetailRepository> _mockServiceSampleFabricWashDetailRepository;

        public PlaceGarmentServiceSampleFabricWashCommandHandlerTests()
        {
            _mockServiceSampleFabricWashRepository = CreateMock<IGarmentServiceSampleFabricWashRepository>();
            _mockServiceSampleFabricWashItemRepository = CreateMock<IGarmentServiceSampleFabricWashItemRepository>();
            _mockServiceSampleFabricWashDetailRepository = CreateMock<IGarmentServiceSampleFabricWashDetailRepository>();

            _MockStorage.SetupStorage(_mockServiceSampleFabricWashRepository);
            _MockStorage.SetupStorage(_mockServiceSampleFabricWashItemRepository);
            _MockStorage.SetupStorage(_mockServiceSampleFabricWashDetailRepository);
        }

        private PlaceGarmentServiceSampleFabricWashCommandHandler CreatePlaceGarmentServiceSampleFabricWashCommandHandler()
        {
            return new PlaceGarmentServiceSampleFabricWashCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            PlaceGarmentServiceSampleFabricWashCommandHandler unitUnderTest = CreatePlaceGarmentServiceSampleFabricWashCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentServiceSampleFabricWashCommand placeGarmentServiceSampleFabricWashCommand = new PlaceGarmentServiceSampleFabricWashCommand()
            {
                ServiceSampleFabricWashDate = DateTimeOffset.Now,
                IsSave = true,
                Items = new List<GarmentServiceSampleFabricWashItemValueObject>
                {
                    new GarmentServiceSampleFabricWashItemValueObject
                    {
                        ExpenditureDate = DateTimeOffset.Now,
                        UnitExpenditureNo = "no",
                        UnitRequest = new UnitRequest(1, "UnitRequestCode", "UnitRequestName"),
                        UnitSender = new UnitSender(1, "UnitSenderCode", "UnitSenderName"),
                        Details = new List<GarmentServiceSampleFabricWashDetailValueObject>
                        {
                            new GarmentServiceSampleFabricWashDetailValueObject
                            {
                                Product = new Product(1, "ProductCode", "ProductName"),
                                Uom = new Uom(1, "UomUnit"),
                                IsSave=true,
                                Quantity=1,
                                DesignColor= "DesignColor"
                            }
                        }
                    }
                },

            };
            
            _mockServiceSampleFabricWashRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSampleFabricWashReadModel>().AsQueryable());


            _mockServiceSampleFabricWashRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSampleFabricWash>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSampleFabricWash>()));
            _mockServiceSampleFabricWashItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSampleFabricWashItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSampleFabricWashItem>()));
            _mockServiceSampleFabricWashDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSampleFabricWashDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSampleFabricWashDetail>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(placeGarmentServiceSampleFabricWashCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
