using Barebone.Tests;
using Manufactures.Application.GarmentSample.GarmentServiceSampleCuttings.CommandHandlers;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings.Commands;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings.ReadModels;
using System.Linq.Expressions;
using System.Linq;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings;
using FluentAssertions;

namespace Manufactures.Tests.CommandHandlers.GarmentSample.GarmentServiceSampleCuttings
{
    public class RemoveGarmentServiceSampleCuttingCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentServiceSampleCuttingRepository> _mockServiceSampleCuttingRepository;
        private readonly Mock<IGarmentServiceSampleCuttingItemRepository> _mockServiceSampleCuttingItemRepository;
        private readonly Mock<IGarmentServiceSampleCuttingDetailRepository> _mockServiceSampleCuttingDetailRepository;
        private readonly Mock<IGarmentServiceSampleCuttingSizeRepository> _mockServiceSampleCuttingSizeRepository;

        public RemoveGarmentServiceSampleCuttingCommandHandlerTests()
        {
            _mockServiceSampleCuttingRepository = CreateMock<IGarmentServiceSampleCuttingRepository>();
            _mockServiceSampleCuttingItemRepository = CreateMock<IGarmentServiceSampleCuttingItemRepository>();
            _mockServiceSampleCuttingDetailRepository = CreateMock<IGarmentServiceSampleCuttingDetailRepository>();
            _mockServiceSampleCuttingSizeRepository = CreateMock<IGarmentServiceSampleCuttingSizeRepository>();

            _MockStorage.SetupStorage(_mockServiceSampleCuttingRepository);
            _MockStorage.SetupStorage(_mockServiceSampleCuttingItemRepository);
            _MockStorage.SetupStorage(_mockServiceSampleCuttingDetailRepository);
            _MockStorage.SetupStorage(_mockServiceSampleCuttingSizeRepository);
        }
        private RemoveGarmentServiceSampleCuttingCommandHandler CreateRemoveGarmentServiceSampleCuttingCommandHandler()
        {
            return new RemoveGarmentServiceSampleCuttingCommandHandler(_MockStorage.Object);
        }
        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid ServiceSampleCuttingItemGuid = Guid.NewGuid();
            Guid ServiceSampleCuttingGuid = Guid.NewGuid();
            Guid SampleCuttingDetailGuid = Guid.NewGuid();
            RemoveGarmentServiceSampleCuttingCommandHandler unitUnderTest = CreateRemoveGarmentServiceSampleCuttingCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentServiceSampleCuttingCommand RemoveGarmentServiceSampleCuttingCommand = new RemoveGarmentServiceSampleCuttingCommand(ServiceSampleCuttingGuid);

            _mockServiceSampleCuttingRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSampleCuttingReadModel>()
                {
                    new GarmentServiceSampleCuttingReadModel(ServiceSampleCuttingGuid)
                }.AsQueryable());
            _mockServiceSampleCuttingItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSampleCuttingItemReadModel, bool>>>()))
                .Returns(new List<GarmentServiceSampleCuttingItem>()
                {
                    new GarmentServiceSampleCuttingItem(ServiceSampleCuttingItemGuid, ServiceSampleCuttingGuid,null,null,new GarmentComodityId(1),null,null)
                });
            _mockServiceSampleCuttingDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSampleCuttingDetailReadModel, bool>>>()))
                .Returns(new List<GarmentServiceSampleCuttingDetail>()
                {
                    new GarmentServiceSampleCuttingDetail(SampleCuttingDetailGuid, ServiceSampleCuttingItemGuid,  "ColorD", 1)
                });
            _mockServiceSampleCuttingSizeRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSampleCuttingSizeReadModel, bool>>>()))
                .Returns(new List<GarmentServiceSampleCuttingSize>()
                {
                    new GarmentServiceSampleCuttingSize(new Guid(),new SizeId(1),"",1,new UomId(1),"", "ColorD", SampleCuttingDetailGuid, Guid.Empty, Guid.Empty,new ProductId(1), "", "")
                });

            _mockServiceSampleCuttingRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSampleCutting>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSampleCutting>()));
            _mockServiceSampleCuttingItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSampleCuttingItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSampleCuttingItem>()));
            _mockServiceSampleCuttingDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSampleCuttingDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSampleCuttingDetail>()));
            _mockServiceSampleCuttingSizeRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSampleCuttingSize>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSampleCuttingSize>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(RemoveGarmentServiceSampleCuttingCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
