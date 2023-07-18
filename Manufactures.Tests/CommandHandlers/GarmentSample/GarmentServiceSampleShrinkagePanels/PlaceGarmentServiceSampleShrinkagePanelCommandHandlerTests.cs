using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.GarmentServiceSampleShrinkagePanels.CommandHandlers;
using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels;
using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels.ReadModels;
using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels.Repositories;
using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSample.GarmentServiceSampleShrinkagePanels
{
    public class PlaceGarmentServiceSampleShrinkagePanelCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentServiceSampleShrinkagePanelRepository> _mockServiceSampleShrinkagePanelRepository;
        private readonly Mock<IGarmentServiceSampleShrinkagePanelItemRepository> _mockServiceSampleShrinkagePanelItemRepository;
        private readonly Mock<IGarmentServiceSampleShrinkagePanelDetailRepository> _mockServiceSampleShrinkagePanelDetailRepository;

        public PlaceGarmentServiceSampleShrinkagePanelCommandHandlerTests()
        {
            _mockServiceSampleShrinkagePanelRepository = CreateMock<IGarmentServiceSampleShrinkagePanelRepository>();
            _mockServiceSampleShrinkagePanelItemRepository = CreateMock<IGarmentServiceSampleShrinkagePanelItemRepository>();
            _mockServiceSampleShrinkagePanelDetailRepository = CreateMock<IGarmentServiceSampleShrinkagePanelDetailRepository>();

            _MockStorage.SetupStorage(_mockServiceSampleShrinkagePanelRepository);
            _MockStorage.SetupStorage(_mockServiceSampleShrinkagePanelItemRepository);
            _MockStorage.SetupStorage(_mockServiceSampleShrinkagePanelDetailRepository);
        }

        private PlaceGarmentServiceSampleShrinkagePanelCommandHandler CreatePlaceGarmentServiceSampleShrinkagePanelCommandHandler()
        {
            return new PlaceGarmentServiceSampleShrinkagePanelCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            PlaceGarmentServiceSampleShrinkagePanelCommandHandler unitUnderTest = CreatePlaceGarmentServiceSampleShrinkagePanelCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentServiceSampleShrinkagePanelCommand placeGarmentServiceSampleShrinkagePanelCommand = new PlaceGarmentServiceSampleShrinkagePanelCommand()
            {
                ServiceSampleShrinkagePanelDate = DateTimeOffset.Now,
                IsSave = true,
                Items = new List<GarmentServiceSampleShrinkagePanelItemValueObject>
                {
                    new GarmentServiceSampleShrinkagePanelItemValueObject
                    {
                        ExpenditureDate = DateTimeOffset.Now,
                        UnitExpenditureNo = "no",
                        UnitRequest = new UnitRequest(1, "UnitRequestCode", "UnitRequestName"),
                        UnitSender = new UnitSender(1, "UnitSenderCode", "UnitSenderName"),
                        Details = new List<GarmentServiceSampleShrinkagePanelDetailValueObject>
                        {
                            new GarmentServiceSampleShrinkagePanelDetailValueObject
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
            
            _mockServiceSampleShrinkagePanelRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSampleShrinkagePanelReadModel>().AsQueryable());


            _mockServiceSampleShrinkagePanelRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSampleShrinkagePanel>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSampleShrinkagePanel>()));
            _mockServiceSampleShrinkagePanelItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSampleShrinkagePanelItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSampleShrinkagePanelItem>()));
            _mockServiceSampleShrinkagePanelDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSampleShrinkagePanelDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSampleShrinkagePanelDetail>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(placeGarmentServiceSampleShrinkagePanelCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
