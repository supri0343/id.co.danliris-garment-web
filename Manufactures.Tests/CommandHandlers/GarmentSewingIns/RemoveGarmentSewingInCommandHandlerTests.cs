using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSewingIns.CommandHandlers;
using Manufactures.Domain.GarmentSewingIns;
using Manufactures.Domain.GarmentSewingIns.Commands;
using Manufactures.Domain.GarmentSewingIns.ReadModels;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using Manufactures.Domain.GarmentSewingIns.ValueObjects;
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


namespace Manufactures.Tests.CommandHandlers.GarmentSewingIns
{
    public class RemoveGarmentSewingInCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSewingInRepository> _mockSewingInRepository;
        private readonly Mock<IGarmentSewingInItemRepository> _mockSewingInItemRepository;

        public RemoveGarmentSewingInCommandHandlerTests()
        {
            _mockSewingInRepository = CreateMock<IGarmentSewingInRepository>();
            _mockSewingInItemRepository = CreateMock<IGarmentSewingInItemRepository>();

            _MockStorage.SetupStorage(_mockSewingInRepository);
            _MockStorage.SetupStorage(_mockSewingInItemRepository);
        }

        private RemoveGarmentSewingInCommandHandler CreateRemoveGarmentSewingInCommandHandler()
        {
            return new RemoveGarmentSewingInCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid SewingInGuid = Guid.NewGuid();
            Guid preparingItemGuid = Guid.NewGuid();
            RemoveGarmentSewingInCommandHandler unitUnderTest = CreateRemoveGarmentSewingInCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentSewingInCommand RemoveGarmentSewingInCommand = new RemoveGarmentSewingInCommand(SewingInGuid);

            _mockSewingInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSewingInReadModel>()
                {
                    new GarmentSewingInReadModel(SewingInGuid)
                }.AsQueryable());
            _mockSewingInItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSewingInItemReadModel, bool>>>()))
                .Returns(new List<GarmentSewingInItem>()
                {
                    new GarmentSewingInItem(Guid.Empty, Guid.Empty,Guid.Empty,Guid.Empty, Guid.Empty, new ProductId(1), null, null, null, new SizeId(1), null, 0, new UomId(1), null, null, 0)
                });
            
            _mockSewingInRepository
                .Setup(s => s.Update(It.IsAny<GarmentSewingIn>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSewingIn>()));
            _mockSewingInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSewingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSewingInItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(RemoveGarmentSewingInCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}