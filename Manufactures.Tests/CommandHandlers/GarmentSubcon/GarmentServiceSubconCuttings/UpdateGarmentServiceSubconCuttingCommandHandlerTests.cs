using Barebone.Tests;
using Manufactures.Application.GarmentSubcon.GarmentServiceSubconCuttings.CommandHandlers;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.ValueObjects;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.ReadModels;
using System.Linq.Expressions;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings;
using System.Linq;
using FluentAssertions;

namespace Manufactures.Tests.CommandHandlers.GarmentSubcon.GarmentServiceSubconCuttings
{
    public class UpdateGarmentServiceSubconCuttingCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentServiceSubconCuttingRepository> _mockServiceSubconCuttingRepository;
        private readonly Mock<IGarmentServiceSubconCuttingItemRepository> _mockServiceSubconCuttingItemRepository;
        
        public UpdateGarmentServiceSubconCuttingCommandHandlerTests()
        {
            _mockServiceSubconCuttingRepository = CreateMock<IGarmentServiceSubconCuttingRepository>();
            _mockServiceSubconCuttingItemRepository = CreateMock<IGarmentServiceSubconCuttingItemRepository>();
            
            _MockStorage.SetupStorage(_mockServiceSubconCuttingRepository);
            _MockStorage.SetupStorage(_mockServiceSubconCuttingItemRepository);
            
        }
        private UpdateGarmentServiceSubconCuttingCommandHandler CreateUpdateGarmentServiceSubconCuttingCommandHandler()
        {
            return new UpdateGarmentServiceSubconCuttingCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid ServiceSubconCuttingGuid = Guid.NewGuid();
            Guid sewingInId = Guid.NewGuid();
            UpdateGarmentServiceSubconCuttingCommandHandler unitUnderTest = CreateUpdateGarmentServiceSubconCuttingCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateGarmentServiceSubconCuttingCommand UpdateGarmentServiceSubconCuttingCommand = new UpdateGarmentServiceSubconCuttingCommand()
            {
                RONo = "RONo",
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                Article = "Article",
                Comodity = new GarmentComodity(1, "ComoCode", "ComoName"),
                SubconDate = DateTimeOffset.Now,
                SubconType = "BORDIR",
                Items = new List<GarmentServiceSubconCuttingItemValueObject>
                {
                    new GarmentServiceSubconCuttingItemValueObject
                    {
                        Product = new Product(1, "ProductCode", "ProductName"),
                        IsSave=true,
                        Quantity=1,
                        DesignColor= "ColorD",
                        CuttingInQuantity=1,
                        CuttingInDetailId=Guid.NewGuid(),
                        ServiceSubconCuttingId=Guid.NewGuid()
                    }
                },

            };
            UpdateGarmentServiceSubconCuttingCommand.SetIdentity(ServiceSubconCuttingGuid);

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
                    new GarmentServiceSubconCuttingItem(Guid.Empty, ServiceSubconCuttingGuid, Guid.Empty,new ProductId(1),null,null,null, 1)
                });

            _mockServiceSubconCuttingRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconCutting>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconCutting>()));
            _mockServiceSubconCuttingItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconCuttingItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconCuttingItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(UpdateGarmentServiceSubconCuttingCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
