using Barebone.Tests;
using Manufactures.Application.GarmentFinishingOuts.CommandHandlers;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GarmentFinishingOuts;
using Manufactures.Domain.GarmentFinishingOuts.ReadModels;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
using Manufactures.Domain.GarmentFinishingOuts.Commands;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.GarmentFinishingIns.ReadModels;
using Manufactures.Domain.GarmentFinishingIns;
using FluentAssertions;

namespace Manufactures.Tests.CommandHandlers.GarmentFinishingOuts
{
    public class RemoveGarmentFinishingOutCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentFinishingOutRepository> _mockFinishingOutRepository;
        private readonly Mock<IGarmentFinishingOutItemRepository> _mockFinishingOutItemRepository;
        private readonly Mock<IGarmentFinishingOutDetailRepository> _mockFinishingOutDetailRepository;
        private readonly Mock<IGarmentFinishingInItemRepository> _mockFinishingInItemRepository;

        public RemoveGarmentFinishingOutCommandHandlerTests()
        {
            _mockFinishingOutRepository = CreateMock<IGarmentFinishingOutRepository>();
            _mockFinishingOutItemRepository = CreateMock<IGarmentFinishingOutItemRepository>();
            _mockFinishingOutDetailRepository = CreateMock<IGarmentFinishingOutDetailRepository>();
            _mockFinishingInItemRepository = CreateMock<IGarmentFinishingInItemRepository>();

            _MockStorage.SetupStorage(_mockFinishingOutRepository);
            _MockStorage.SetupStorage(_mockFinishingOutItemRepository);
            _MockStorage.SetupStorage(_mockFinishingOutDetailRepository);
            _MockStorage.SetupStorage(_mockFinishingInItemRepository);
        }
        private RemoveGarmentFinishingOutCommandHandler CreateRemoveGarmentFinishingOutCommandHandler()
        {
            return new RemoveGarmentFinishingOutCommandHandler(_MockStorage.Object);
        }
        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid finishingInItemGuid = Guid.NewGuid();
            Guid finishingOutGuid = Guid.NewGuid();
            RemoveGarmentFinishingOutCommandHandler unitUnderTest = CreateRemoveGarmentFinishingOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentFinishingOutCommand RemoveGarmentFinishingOutCommand = new RemoveGarmentFinishingOutCommand(finishingOutGuid);

            _mockFinishingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentFinishingOutReadModel>()
                {
                    new GarmentFinishingOutReadModel(finishingOutGuid)
                }.AsQueryable());
            _mockFinishingOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentFinishingOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentFinishingOutItem>()
                {
                    new GarmentFinishingOutItem(Guid.Empty, finishingOutGuid, Guid.Empty,finishingInItemGuid,new ProductId(1),null,null,null,new SizeId(1), null, 1, new UomId(1), null,null, 1,1,1)
                });
            //_mockFinishingOutDetailRepository
            //    .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentFinishingOutDetailReadModel, bool>>>()))
            //    .Returns(new List<GarmentFinishingOutDetail>()
            //    {
            //        new GarmentFinishingOutDetail(Guid.Empty, Guid.Empty,new SizeId(1), null, 1, new UomId(1),null )
            //    });

            _mockFinishingInItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentFinishingInItemReadModel>
                {
                    new GarmentFinishingInItemReadModel(finishingInItemGuid)
                }.AsQueryable());

            _mockFinishingOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentFinishingOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentFinishingOut>()));
            _mockFinishingOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentFinishingOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentFinishingOutItem>()));
            //_mockFinishingOutDetailRepository
            //    .Setup(s => s.Update(It.IsAny<GarmentFinishingOutDetail>()))
            //    .Returns(Task.FromResult(It.IsAny<GarmentFinishingOutDetail>()));
            _mockFinishingInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentFinishingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentFinishingInItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(RemoveGarmentFinishingOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
