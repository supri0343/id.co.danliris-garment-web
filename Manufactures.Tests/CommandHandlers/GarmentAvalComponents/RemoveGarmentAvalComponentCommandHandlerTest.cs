using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentAvalComponents.CommandHandlers;
using Manufactures.Domain.GarmentAvalComponents;
using Manufactures.Domain.GarmentAvalComponents.Commands;
using Manufactures.Domain.GarmentAvalComponents.ReadModels;
using Manufactures.Domain.GarmentAvalComponents.Repositories;
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

namespace Manufactures.Tests.CommandHandlers.GarmentAvalComponents
{
    public class RemoveGarmentAvalComponentCommandHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentAvalComponentRepository> _mockGarmentAvalComponentRepository;
        private readonly Mock<IGarmentAvalComponentItemRepository> _mockGarmentAvalComponentItemRepository;

        public RemoveGarmentAvalComponentCommandHandlerTest()
        {
            _mockGarmentAvalComponentRepository = CreateMock<IGarmentAvalComponentRepository>();
            _mockGarmentAvalComponentItemRepository = CreateMock<IGarmentAvalComponentItemRepository>();

            _MockStorage.SetupStorage(_mockGarmentAvalComponentRepository);
            _MockStorage.SetupStorage(_mockGarmentAvalComponentItemRepository);
        }

        private RemoveGarmentAvalComponentCommandHandler CreateRemoveGarmentAvalComponentCommandHandler()
        {
            return new RemoveGarmentAvalComponentCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            RemoveGarmentAvalComponentCommandHandler unitUnderTest = CreateRemoveGarmentAvalComponentCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;

            Guid avalComponentGuid = Guid.NewGuid();

            RemoveGarmentAvalComponentCommand removeGarmentAvalComponentCommand = new RemoveGarmentAvalComponentCommand(avalComponentGuid);

            _mockGarmentAvalComponentRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentAvalComponentReadModel>()
                {
                    new GarmentAvalComponentReadModel(avalComponentGuid)
                }.AsQueryable());
            _mockGarmentAvalComponentItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentAvalComponentItemReadModel, bool>>>()))
                .Returns(new List<GarmentAvalComponentItem>()
                {
                    new GarmentAvalComponentItem(Guid.Empty, avalComponentGuid, Guid.Empty, Guid.Empty, Guid.Empty, new ProductId(1), null, null, null, null, 0, 0, new SizeId(1), null, 0,1)
                });

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
            var result = await unitUnderTest.Handle(removeGarmentAvalComponentCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
