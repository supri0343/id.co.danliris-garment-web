using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.GarmentServiceSampleShrinkagePanels.CommandHandlers;
using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels;
using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels.ReadModels;
using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSample.GarmentServiceSampleShrinkagePanels
{
    public class RemoveGarmentServiceSampleShrinkagePanelCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentServiceSampleShrinkagePanelRepository> _mockServiceSampleShrinkagePanelRepository;
        private readonly Mock<IGarmentServiceSampleShrinkagePanelItemRepository> _mockServiceSampleShrinkagePanelItemRepository;
        private readonly Mock<IGarmentServiceSampleShrinkagePanelDetailRepository> _mockServiceSampleShrinkagePanelDetailRepository;

        public RemoveGarmentServiceSampleShrinkagePanelCommandHandlerTests()
        {
            _mockServiceSampleShrinkagePanelRepository = CreateMock<IGarmentServiceSampleShrinkagePanelRepository>();
            _mockServiceSampleShrinkagePanelItemRepository = CreateMock<IGarmentServiceSampleShrinkagePanelItemRepository>();
            _mockServiceSampleShrinkagePanelDetailRepository = CreateMock<IGarmentServiceSampleShrinkagePanelDetailRepository>();

            _MockStorage.SetupStorage(_mockServiceSampleShrinkagePanelRepository);
            _MockStorage.SetupStorage(_mockServiceSampleShrinkagePanelItemRepository);
            _MockStorage.SetupStorage(_mockServiceSampleShrinkagePanelDetailRepository);
        }

        private RemoveGarmentServiceSampleShrinkagePanelCommandHandler CreateRemoveGarmentServiceSampleShrinkagePanelCommandHandler()
        {
            return new RemoveGarmentServiceSampleShrinkagePanelCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid serviceSampleShrinkagePanelGuid = Guid.NewGuid();
            Guid serviceSampleShrinkagePanelItemGuid = Guid.NewGuid();
            RemoveGarmentServiceSampleShrinkagePanelCommandHandler unitUnderTest = CreateRemoveGarmentServiceSampleShrinkagePanelCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentServiceSampleShrinkagePanelCommand RemoveGarmentServiceSampleShrinkagePanelCommand = new RemoveGarmentServiceSampleShrinkagePanelCommand(serviceSampleShrinkagePanelGuid);

            _mockServiceSampleShrinkagePanelRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSampleShrinkagePanelReadModel>()
                {
                    new GarmentServiceSampleShrinkagePanelReadModel(serviceSampleShrinkagePanelGuid)
                }.AsQueryable());

            _mockServiceSampleShrinkagePanelItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSampleShrinkagePanelItemReadModel, bool>>>()))
                .Returns(new List<GarmentServiceSampleShrinkagePanelItem>()
                {
                    new GarmentServiceSampleShrinkagePanelItem(
                        serviceSampleShrinkagePanelItemGuid,
                        serviceSampleShrinkagePanelGuid,
                        null,
                        DateTimeOffset.Now,
                        new UnitSenderId(1),
                        null,
                        null,
                        new UnitRequestId(1),
                        null,
                        null)
                });
            _mockServiceSampleShrinkagePanelDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSampleShrinkagePanelDetailReadModel, bool>>>()))
                .Returns(new List<GarmentServiceSampleShrinkagePanelDetail>()
                {
                    new GarmentServiceSampleShrinkagePanelDetail(
                        new Guid(),
                        serviceSampleShrinkagePanelItemGuid,
                        new ProductId(1),
                        null,
                        null,
                        null,
                        null,
                        1,
                        new UomId(1),
                        null)
                });

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
            var result = await unitUnderTest.Handle(RemoveGarmentServiceSampleShrinkagePanelCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
