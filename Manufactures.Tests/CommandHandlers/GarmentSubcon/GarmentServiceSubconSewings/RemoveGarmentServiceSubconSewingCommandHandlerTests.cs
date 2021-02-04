using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSubcon.GarmentServiceSubconSewings.CommandHandlers;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSubcon.GarmentServiceSubconSewings
{
    public class RemoveGarmentServiceSubconSewingCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentServiceSubconSewingRepository> _mockServiceSubconSewingRepository;
        private readonly Mock<IGarmentServiceSubconSewingItemRepository> _mockServiceSubconSewingItemRepository;

        public RemoveGarmentServiceSubconSewingCommandHandlerTests()
        {
            _mockServiceSubconSewingRepository = CreateMock<IGarmentServiceSubconSewingRepository>();
            _mockServiceSubconSewingItemRepository = CreateMock<IGarmentServiceSubconSewingItemRepository>();

            _MockStorage.SetupStorage(_mockServiceSubconSewingRepository);
            _MockStorage.SetupStorage(_mockServiceSubconSewingItemRepository);
        }

        private RemoveGarmentServiceSubconSewingCommandHandler CreateRemoveGarmentServiceSubconSewingCommandHandler()
        {
            return new RemoveGarmentServiceSubconSewingCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid sewingInItemGuid = Guid.NewGuid();
            Guid serviceSubconSewingGuid = Guid.NewGuid();
            RemoveGarmentServiceSubconSewingCommandHandler unitUnderTest = CreateRemoveGarmentServiceSubconSewingCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentServiceSubconSewingCommand RemoveGarmentServiceSubconSewingCommand = new RemoveGarmentServiceSubconSewingCommand(serviceSubconSewingGuid);

            _mockServiceSubconSewingRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconSewingReadModel>()
                {
                    new GarmentServiceSubconSewingReadModel(serviceSubconSewingGuid)
                }.AsQueryable());

            _mockServiceSubconSewingItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSubconSewingItemReadModel, bool>>>()))
                .Returns(new List<GarmentServiceSubconSewingItem>()
                {
                    new GarmentServiceSubconSewingItem(
                        Guid.Empty,
                        serviceSubconSewingGuid,
                        null,
                        null,
                        new GarmentComodityId(1),
                        null,
                        null)
                });


            _mockServiceSubconSewingRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconSewing>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconSewing>()));
            _mockServiceSubconSewingItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconSewingItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconSewingItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(RemoveGarmentServiceSubconSewingCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
