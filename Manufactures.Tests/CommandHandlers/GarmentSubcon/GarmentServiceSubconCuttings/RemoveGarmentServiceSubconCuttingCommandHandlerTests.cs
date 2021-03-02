using Barebone.Tests;
using Manufactures.Application.GarmentSubcon.GarmentServiceSubconCuttings.CommandHandlers;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.Commands;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.ReadModels;
using System.Linq.Expressions;
using System.Linq;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings;
using FluentAssertions;

namespace Manufactures.Tests.CommandHandlers.GarmentSubcon.GarmentServiceSubconCuttings
{
    public class RemoveGarmentServiceSubconCuttingCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentServiceSubconCuttingRepository> _mockServiceSubconCuttingRepository;
        private readonly Mock<IGarmentServiceSubconCuttingItemRepository> _mockServiceSubconCuttingItemRepository;
        private readonly Mock<IGarmentServiceSubconCuttingDetailRepository> _mockServiceSubconCuttingDetailRepository;

        public RemoveGarmentServiceSubconCuttingCommandHandlerTests()
        {
            _mockServiceSubconCuttingRepository = CreateMock<IGarmentServiceSubconCuttingRepository>();
            _mockServiceSubconCuttingItemRepository = CreateMock<IGarmentServiceSubconCuttingItemRepository>();
            _mockServiceSubconCuttingDetailRepository = CreateMock<IGarmentServiceSubconCuttingDetailRepository>();

            _MockStorage.SetupStorage(_mockServiceSubconCuttingRepository);
            _MockStorage.SetupStorage(_mockServiceSubconCuttingItemRepository);
            _MockStorage.SetupStorage(_mockServiceSubconCuttingDetailRepository);
        }
        private RemoveGarmentServiceSubconCuttingCommandHandler CreateRemoveGarmentServiceSubconCuttingCommandHandler()
        {
            return new RemoveGarmentServiceSubconCuttingCommandHandler(_MockStorage.Object);
        }
        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid ServiceSubconCuttingItemGuid = Guid.NewGuid();
            Guid ServiceSubconCuttingGuid = Guid.NewGuid();
            RemoveGarmentServiceSubconCuttingCommandHandler unitUnderTest = CreateRemoveGarmentServiceSubconCuttingCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentServiceSubconCuttingCommand RemoveGarmentServiceSubconCuttingCommand = new RemoveGarmentServiceSubconCuttingCommand(ServiceSubconCuttingGuid);

            _mockServiceSubconCuttingRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconCuttingReadModel>()
                {
                    new GarmentServiceSubconCuttingReadModel(ServiceSubconCuttingGuid)
                }.AsQueryable());
            _mockServiceSubconCuttingItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSubconCuttingItemReadModel, bool>>>()))
                .Returns(new List<GarmentServiceSubconCuttingItem>()
                {
                    new GarmentServiceSubconCuttingItem(ServiceSubconCuttingItemGuid, ServiceSubconCuttingGuid,null,null,new GarmentComodityId(1),null,null)
                });
            _mockServiceSubconCuttingDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSubconCuttingDetailReadModel, bool>>>()))
                .Returns(new List<GarmentServiceSubconCuttingDetail>()
                {
                    new GarmentServiceSubconCuttingDetail(Guid.Empty, ServiceSubconCuttingItemGuid, new Guid(),new Guid(),new ProductId(1), "", "", "ColorD", 1)
                });

            _mockServiceSubconCuttingRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconCutting>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconCutting>()));
            _mockServiceSubconCuttingItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconCuttingItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconCuttingItem>()));
            _mockServiceSubconCuttingDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconCuttingDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconCuttingDetail>()));
            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(RemoveGarmentServiceSubconCuttingCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
