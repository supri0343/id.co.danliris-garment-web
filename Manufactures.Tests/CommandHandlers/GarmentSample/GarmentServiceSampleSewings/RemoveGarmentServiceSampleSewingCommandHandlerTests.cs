using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.GarmentServiceSampleSewings.CommandHandlers;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.ReadModels;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSample.GarmentServiceSampleSewings
{
    public class RemoveGarmentServiceSampleSewingCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentServiceSampleSewingRepository> _mockServiceSampleSewingRepository;
        private readonly Mock<IGarmentServiceSampleSewingItemRepository> _mockServiceSampleSewingItemRepository;
        private readonly Mock<IGarmentServiceSampleSewingDetailRepository> _mockServiceSampleSewingDetailRepository;

        public RemoveGarmentServiceSampleSewingCommandHandlerTests()
        {
            _mockServiceSampleSewingRepository = CreateMock<IGarmentServiceSampleSewingRepository>();
            _mockServiceSampleSewingItemRepository = CreateMock<IGarmentServiceSampleSewingItemRepository>();
            _mockServiceSampleSewingDetailRepository = CreateMock<IGarmentServiceSampleSewingDetailRepository>();

            _MockStorage.SetupStorage(_mockServiceSampleSewingRepository);
            _MockStorage.SetupStorage(_mockServiceSampleSewingItemRepository);
            _MockStorage.SetupStorage(_mockServiceSampleSewingDetailRepository);
        }

        private RemoveGarmentServiceSampleSewingCommandHandler CreateRemoveGarmentServiceSampleSewingCommandHandler()
        {
            return new RemoveGarmentServiceSampleSewingCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid sewingInItemGuid = Guid.NewGuid();
            Guid serviceSampleSewingGuid = Guid.NewGuid();
            Guid serviceSampleSewingItemGuid = Guid.NewGuid();
            RemoveGarmentServiceSampleSewingCommandHandler unitUnderTest = CreateRemoveGarmentServiceSampleSewingCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentServiceSampleSewingCommand RemoveGarmentServiceSampleSewingCommand = new RemoveGarmentServiceSampleSewingCommand(serviceSampleSewingGuid);

            _mockServiceSampleSewingRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSampleSewingReadModel>()
                {
                    new GarmentServiceSampleSewingReadModel(serviceSampleSewingGuid)
                }.AsQueryable());

            _mockServiceSampleSewingItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSampleSewingItemReadModel, bool>>>()))
                .Returns(new List<GarmentServiceSampleSewingItem>()
                {
                    new GarmentServiceSampleSewingItem(
                        serviceSampleSewingItemGuid,
                        serviceSampleSewingGuid,
                        null,
                        null,
                        new GarmentComodityId(1),
                        null,
                        null,
                        new BuyerId(1),
                        null,
                        null,
                        new UnitDepartmentId(1),
                        null,
                        null)
                });
            _mockServiceSampleSewingDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSampleSewingDetailReadModel, bool>>>()))
                .Returns(new List<GarmentServiceSampleSewingDetail>()
                {
                    new GarmentServiceSampleSewingDetail(
                        new Guid(),
                        serviceSampleSewingItemGuid,
                        Guid.Empty,
                        Guid.Empty,
                        new ProductId(1),
                        null,
                        null,
                        null,
                        1,
                        new UomId(1),
                        null,
                        new UnitDepartmentId(1),
                        null,
                        null,
                        null,
                        null)
                });

            _mockServiceSampleSewingRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSampleSewing>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSampleSewing>()));
            _mockServiceSampleSewingItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSampleSewingItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSampleSewingItem>()));
            _mockServiceSampleSewingDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSampleSewingDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSampleSewingDetail>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(RemoveGarmentServiceSampleSewingCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
