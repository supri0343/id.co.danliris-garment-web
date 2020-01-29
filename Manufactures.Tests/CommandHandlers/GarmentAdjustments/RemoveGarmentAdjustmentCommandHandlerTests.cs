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
using Manufactures.Domain.GarmentSewingIns;
using Manufactures.Domain.GarmentSewingIns.ReadModels;
using Manufactures.Domain.GarmentSewingIns.Repositories;
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
        private readonly Mock<IGarmentSewingInItemRepository> _mockSewingInItemRepository;

        public RemoveGarmentAdjustmentCommandHandlerTests()
        {
            _mockAdjustmentRepository = CreateMock<IGarmentAdjustmentRepository>();
            _mockAdjustmentItemRepository = CreateMock<IGarmentAdjustmentItemRepository>();
            _mockSewingDOItemRepository = CreateMock<IGarmentSewingDOItemRepository>();
            _mockSewingInItemRepository = CreateMock<IGarmentSewingInItemRepository>();

            _MockStorage.SetupStorage(_mockAdjustmentRepository);
            _MockStorage.SetupStorage(_mockAdjustmentItemRepository);
            _MockStorage.SetupStorage(_mockSewingDOItemRepository);
            _MockStorage.SetupStorage(_mockSewingInItemRepository);
        }

        private RemoveGarmentAdjustmentCommandHandler CreateRemoveGarmentAdjustmentCommandHandler()
        {
            return new RemoveGarmentAdjustmentCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_LOADING()
        {
            // Arrange
            Guid adjustmentGuid = Guid.NewGuid();
            Guid sewingDOItemGuid = Guid.NewGuid();
            Guid sewingInItemGuid = Guid.NewGuid();
            Guid sewingDOGuid = Guid.NewGuid();
            RemoveGarmentAdjustmentCommandHandler unitUnderTest = CreateRemoveGarmentAdjustmentCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentAdjustmentCommand RemoveGarmentAdjustmentCommand = new RemoveGarmentAdjustmentCommand(adjustmentGuid);

            GarmentAdjustment garmentAdjustment = new GarmentAdjustment(
                adjustmentGuid,null,"LOADING","roNo",null,new UnitDepartmentId(1), null, null, DateTimeOffset.Now,
                new GarmentComodityId(1), null, null);

            _mockAdjustmentRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentAdjustmentReadModel>()
                {
                    garmentAdjustment.GetReadModel()
                }.AsQueryable());

            _mockAdjustmentItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentAdjustmentItemReadModel, bool>>>()))
                .Returns(new List<GarmentAdjustmentItem>()
                {
                    new GarmentAdjustmentItem(Guid.Empty, Guid.Empty,sewingDOItemGuid,sewingInItemGuid,Guid.Empty,new SizeId(1), null, new ProductId(1), null, null, null, 1,10,new UomId(1),null, null,1)
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

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_SEWING()
        {
            // Arrange
            Guid adjustmentGuid = Guid.NewGuid();
            Guid sewingDOItemGuid = Guid.NewGuid();
            Guid sewingInItemGuid = Guid.NewGuid();
            Guid sewingDOGuid = Guid.NewGuid();
            RemoveGarmentAdjustmentCommandHandler unitUnderTest = CreateRemoveGarmentAdjustmentCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentAdjustmentCommand RemoveGarmentAdjustmentCommand = new RemoveGarmentAdjustmentCommand(adjustmentGuid);

            GarmentAdjustment garmentAdjustment = new GarmentAdjustment(
                adjustmentGuid, null, "SEWING", "roNo", null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now,
                new GarmentComodityId(1), null, null);

            _mockAdjustmentRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentAdjustmentReadModel>()
                {
                    garmentAdjustment.GetReadModel()
                }.AsQueryable());

            _mockAdjustmentItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentAdjustmentItemReadModel, bool>>>()))
                .Returns(new List<GarmentAdjustmentItem>()
                {
                    new GarmentAdjustmentItem(Guid.Empty, Guid.Empty,sewingDOItemGuid,sewingInItemGuid,Guid.Empty,new SizeId(1), null, new ProductId(1), null, null, null, 1,10,new UomId(1),null, null,1)
                });


            _mockSewingInItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSewingInItemReadModel>
                {
                    new GarmentSewingInItemReadModel(sewingInItemGuid)
                }.AsQueryable());

            _mockAdjustmentRepository
                .Setup(s => s.Update(It.IsAny<GarmentAdjustment>()))
                .Returns(Task.FromResult(It.IsAny<GarmentAdjustment>()));
            _mockAdjustmentItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentAdjustmentItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentAdjustmentItem>()));
            _mockSewingInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSewingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSewingInItem>()));

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