using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentAdjustments.CommandHandlers;
using Manufactures.Domain.GarmentAdjustments;
using Manufactures.Domain.GarmentAdjustments.Commands;
using Manufactures.Domain.GarmentAdjustments.ReadModels;
using Manufactures.Domain.GarmentAdjustments.Repositories;
using Manufactures.Domain.GarmentAdjustments.ValueObjects;
using Manufactures.Domain.GarmentSewingDOs;
using Manufactures.Domain.GarmentSewingDOs.ReadModels;
using Manufactures.Domain.GarmentSewingDOs.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentAdjustments
{
    public class RemoveGarmentAdjustmentCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentAdjustmentRepository> _mockAdjustmentRepository;
        private readonly Mock<IGarmentAdjustmentItemRepository> _mockAdjustmentItemRepository;
        private readonly Mock<IGarmentSewingDOItemRepository> _mockSewingDOItemRepository;

        public RemoveGarmentAdjustmentCommandHandlerTests()
        {
            _mockAdjustmentRepository = CreateMock<IGarmentAdjustmentRepository>();
            _mockAdjustmentItemRepository = CreateMock<IGarmentAdjustmentItemRepository>();
            _mockSewingDOItemRepository = CreateMock<IGarmentSewingDOItemRepository>();

            _MockStorage.SetupStorage(_mockAdjustmentRepository);
            _MockStorage.SetupStorage(_mockAdjustmentItemRepository);
            _MockStorage.SetupStorage(_mockSewingDOItemRepository);
        }

        private RemoveGarmentAdjustmentCommandHandler CreateRemoveGarmentAdjustmentCommandHandler()
        {
            return new RemoveGarmentAdjustmentCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid loadingGuid = Guid.NewGuid();
            Guid sewingDOItemGuid = Guid.NewGuid();
            Guid sewingDOGuid = Guid.NewGuid();
            RemoveGarmentAdjustmentCommandHandler unitUnderTest = CreateRemoveGarmentAdjustmentCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentAdjustmentCommand RemoveGarmentAdjustmentCommand = new RemoveGarmentAdjustmentCommand(loadingGuid);

            _mockAdjustmentRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentAdjustmentReadModel>()
                {
                    new GarmentAdjustmentReadModel(loadingGuid)
                }.AsQueryable());
            _mockAdjustmentItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentAdjustmentItemReadModel, bool>>>()))
                .Returns(new List<GarmentAdjustmentItem>()
                {
                    new GarmentAdjustmentItem(Guid.Empty, Guid.Empty,sewingDOItemGuid,new SizeId(1), null, new ProductId(1), null, null, null, 1,10,new UomId(1),null, null,1)
                });


            _mockSewingDOItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSewingDOItemReadModel>
                {
                    new GarmentSewingDOItemReadModel(sewingDOItemGuid)
                }.AsQueryable());

            _mockAdjustmentRepository
                .Setup(s => s.Update(It.IsAny<GarmentAdjustment>()))
                .Returns(Task.FromResult(It.IsAny<GarmentAdjustment>()));
            _mockAdjustmentItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentAdjustmentItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentAdjustmentItem>()));
            _mockSewingDOItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSewingDOItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSewingDOItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(RemoveGarmentAdjustmentCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}