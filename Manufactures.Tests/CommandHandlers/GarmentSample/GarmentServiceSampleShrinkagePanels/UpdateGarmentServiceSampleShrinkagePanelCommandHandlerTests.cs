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
    public class UpdateGarmentServiceSampleShrinkagePanelCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentServiceSampleShrinkagePanelRepository> _mockServiceSampleShrinkagePanelRepository;
        private readonly Mock<IGarmentServiceSampleShrinkagePanelItemRepository> _mockServiceSampleShrinkagePanelItemRepository;

        public UpdateGarmentServiceSampleShrinkagePanelCommandHandlerTests()
        {
            _mockServiceSampleShrinkagePanelRepository = CreateMock<IGarmentServiceSampleShrinkagePanelRepository>();
            _mockServiceSampleShrinkagePanelItemRepository = CreateMock<IGarmentServiceSampleShrinkagePanelItemRepository>();

            _MockStorage.SetupStorage(_mockServiceSampleShrinkagePanelRepository);
            _MockStorage.SetupStorage(_mockServiceSampleShrinkagePanelItemRepository);
        }

        private UpdateGarmentServiceSampleShrinkagePanelCommandHandler CreateUpdateGarmentServiceSampleShrinkagePanelCommandHandler()
        {
            return new UpdateGarmentServiceSampleShrinkagePanelCommandHandler(_MockStorage.Object);
        }

        /*[Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid serviceSampleShrinkagePanelGuid = Guid.NewGuid();
            Guid serviceSampleShrinkagePanelItemGuid = Guid.NewGuid();
            UpdateGarmentServiceSampleShrinkagePanelCommandHandler unitUnderTest = CreateUpdateGarmentServiceSampleShrinkagePanelCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateGarmentServiceSampleShrinkagePanelCommand UpdateGarmentServiceSampleShrinkagePanelCommand = new UpdateGarmentServiceSampleShrinkagePanelCommand()
            {
                Items = new List<GarmentServiceSampleShrinkagePanelItemValueObject>
                {
                    new GarmentServiceSampleShrinkagePanelItemValueObject
                    {
                        UnitExpenditureNo = "unitExpenditureNo",
                        ExpenditureDate = DateTimeOffset.Now,
                        UnitSender = new UnitSender(1, "UnitSenderCode", "UnitSenderName"),
                        UnitRequest = new UnitRequest(1, "UnitRequestCode", "UnitRequestName"),
                        Details = new List<GarmentServiceSampleShrinkagePanelDetailValueObject>
                        {
                            new GarmentServiceSampleShrinkagePanelDetailValueObject
                            {
                                Product = new Product(1, "ProductCode", "ProductName","roductRemark"),
                                Uom = new Uom(1, "UomUnit"),
                                IsSave=true,
                                Quantity=1,
                                DesignColor= "ColorD",
                            }
                        }

                    }
                },

            };

            UpdateGarmentServiceSampleShrinkagePanelCommand.SetIdentity(serviceSampleShrinkagePanelGuid);

            _mockServiceSampleShrinkagePanelRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSampleShrinkagePanelReadModel>()
                {
                    new GarmentServiceSampleShrinkagePanelReadModel(serviceSampleShrinkagePanelGuid)
                }.AsQueryable());

            _mockServiceSampleShrinkagePanelRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSampleShrinkagePanel>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSampleShrinkagePanel>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(UpdateGarmentServiceSampleShrinkagePanelCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }*/
    }
}
