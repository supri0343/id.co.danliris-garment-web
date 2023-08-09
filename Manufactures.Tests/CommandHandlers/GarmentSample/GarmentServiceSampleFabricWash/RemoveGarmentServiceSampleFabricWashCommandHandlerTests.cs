using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.GarmentServiceSampleFabricWashes.CommandHandlers;
using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes;
using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes.ReadModels;
using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes.Repositories;
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

namespace Manufactures.Tests.CommandHandlers.GarmentSample.GarmentServiceSampleFabricWashs
{
    public class RemoveGarmentServiceSampleFabricWashCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentServiceSampleFabricWashRepository> _mockServiceSampleFabricWashRepository;
        private readonly Mock<IGarmentServiceSampleFabricWashItemRepository> _mockServiceSampleFabricWashItemRepository;
        private readonly Mock<IGarmentServiceSampleFabricWashDetailRepository> _mockServiceSampleFabricWashDetailRepository;

        public RemoveGarmentServiceSampleFabricWashCommandHandlerTests()
        {
            _mockServiceSampleFabricWashRepository = CreateMock<IGarmentServiceSampleFabricWashRepository>();
            _mockServiceSampleFabricWashItemRepository = CreateMock<IGarmentServiceSampleFabricWashItemRepository>();
            _mockServiceSampleFabricWashDetailRepository = CreateMock<IGarmentServiceSampleFabricWashDetailRepository>();

            _MockStorage.SetupStorage(_mockServiceSampleFabricWashRepository);
            _MockStorage.SetupStorage(_mockServiceSampleFabricWashItemRepository);
            _MockStorage.SetupStorage(_mockServiceSampleFabricWashDetailRepository);
        }

        private RemoveGarmentServiceSampleFabricWashCommandHandler CreateRemoveGarmentServiceSampleFabricWashCommandHandler()
        {
            return new RemoveGarmentServiceSampleFabricWashCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid serviceSampleFabricWashGuid = Guid.NewGuid();
            Guid serviceSampleFabricWashItemGuid = Guid.NewGuid();
            RemoveGarmentServiceSampleFabricWashCommandHandler unitUnderTest = CreateRemoveGarmentServiceSampleFabricWashCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentServiceSampleFabricWashCommand RemoveGarmentServiceSampleFabricWashCommand = new RemoveGarmentServiceSampleFabricWashCommand(serviceSampleFabricWashGuid);

            _mockServiceSampleFabricWashRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSampleFabricWashReadModel>()
                {
                    new GarmentServiceSampleFabricWashReadModel(serviceSampleFabricWashGuid)
                }.AsQueryable());

            _mockServiceSampleFabricWashItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSampleFabricWashItemReadModel, bool>>>()))
                .Returns(new List<GarmentServiceSampleFabricWashItem>()
                {
                    new GarmentServiceSampleFabricWashItem(
                        serviceSampleFabricWashItemGuid,
                        serviceSampleFabricWashGuid,
                        null,
                        DateTimeOffset.Now,
                        new UnitSenderId(1),
                        null,
                        null,
                        new UnitRequestId(1),
                        null,
                        null)
                });
            _mockServiceSampleFabricWashDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSampleFabricWashDetailReadModel, bool>>>()))
                .Returns(new List<GarmentServiceSampleFabricWashDetail>()
                {
                    new GarmentServiceSampleFabricWashDetail(
                        new Guid(),
                        serviceSampleFabricWashItemGuid,
                        new ProductId(1),
                        null,
                        null,
                        null,
                        null,
                        1,
                        new UomId(1),
                        null)
                });

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
            var result = await unitUnderTest.Handle(RemoveGarmentServiceSampleFabricWashCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
