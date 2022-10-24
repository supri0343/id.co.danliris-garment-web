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
using Manufactures.Domain.GarmentPreparings.Repositories;

namespace Manufactures.Tests.CommandHandlers.GarmentSubcon.GarmentServiceSubconCuttings
{
    public class UpdateGarmentServiceSubconCuttingCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentServiceSubconCuttingRepository> _mockServiceSubconCuttingRepository;
        private readonly Mock<IGarmentServiceSubconCuttingItemRepository> _mockServiceSubconCuttingItemRepository;
        private readonly Mock<IGarmentServiceSubconCuttingDetailRepository> _mockServiceSubconCuttingDetailRepository;
        private readonly Mock<IGarmentServiceSubconCuttingSizeRepository> _mockServiceSubconCuttingSizeRepository;
        //private readonly Mock<IGarmentPreparingRepository> _mockGarmentPreparingRepository;

        public UpdateGarmentServiceSubconCuttingCommandHandlerTests()
        {
            _mockServiceSubconCuttingRepository = CreateMock<IGarmentServiceSubconCuttingRepository>();
            _mockServiceSubconCuttingItemRepository = CreateMock<IGarmentServiceSubconCuttingItemRepository>();
            _mockServiceSubconCuttingDetailRepository = CreateMock<IGarmentServiceSubconCuttingDetailRepository>();
            _mockServiceSubconCuttingSizeRepository = CreateMock<IGarmentServiceSubconCuttingSizeRepository>();
            //_mockGarmentPreparingRepository = CreateMock<IGarmentPreparingRepository>();

            _MockStorage.SetupStorage(_mockServiceSubconCuttingRepository);
            _MockStorage.SetupStorage(_mockServiceSubconCuttingItemRepository);
            _MockStorage.SetupStorage(_mockServiceSubconCuttingDetailRepository);
            _MockStorage.SetupStorage(_mockServiceSubconCuttingSizeRepository);
            //_MockStorage.SetupStorage(_mockGarmentPreparingRepository);
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
            Guid ServiceSubconCuttingItemGuid = Guid.NewGuid();
            UpdateGarmentServiceSubconCuttingCommandHandler unitUnderTest = CreateUpdateGarmentServiceSubconCuttingCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateGarmentServiceSubconCuttingCommand UpdateGarmentServiceSubconCuttingCommand = new UpdateGarmentServiceSubconCuttingCommand()
            {
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                SubconDate = DateTimeOffset.Now,
                SubconType = "PRINT",
                Buyer = new Buyer(1, "BuyerCode", "BuyerName"),
                Uom = new Uom(1, "UomUnit"),
                Items = new List<GarmentServiceSubconCuttingItemValueObject>
                {
                    new GarmentServiceSubconCuttingItemValueObject
                    {

                        Article = "Article",
                        Comodity = new GarmentComodity(1, "ComoCode", "ComoName"),
                        RONo = "RONo",
                        ServiceSubconCuttingId=Guid.NewGuid(),
                        Details= new List<GarmentServiceSubconCuttingDetailValueObject>
                        {
                            new GarmentServiceSubconCuttingDetailValueObject
                            {
                                IsSave=true,
                                Quantity=1,
                                DesignColor= "ColorD",
                                CuttingInQuantity=1,
                                //CuttingInDetailId=Guid.NewGuid(),
                                Sizes = new List<GarmentServiceSubconCuttingSizeValueObject>
                                {
                                    new GarmentServiceSubconCuttingSizeValueObject
                                    {
                                        CuttingInId = Guid.NewGuid(),
                                        CuttingInDetailId = Guid.NewGuid(),
                                        Id = Guid.NewGuid(),
                                        ServiceSubconCuttingDetailId = Guid.NewGuid(),
                                        Size = new SizeValueObject(1, "SizeName"),
                                        Quantity = 1,
                                        Uom = new Uom(1, "PCS"),
                                        Color = "color",
                                        Product = new Product(1, "ProductCode", "ProductName"),
                                    }
                                }
                            }
                        }
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
                    new GarmentServiceSubconCuttingItem(ServiceSubconCuttingItemGuid, ServiceSubconCuttingGuid,null,null,new GarmentComodityId(1),null,null)
                });
            _mockServiceSubconCuttingDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSubconCuttingDetailReadModel, bool>>>()))
                .Returns(new List<GarmentServiceSubconCuttingDetail>()
                {
                    new GarmentServiceSubconCuttingDetail(new Guid(), ServiceSubconCuttingItemGuid, "ColorD", 1)
                });
            _mockServiceSubconCuttingSizeRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSubconCuttingSizeReadModel, bool>>>()))
                .Returns(new List<GarmentServiceSubconCuttingSize>()
                {
                    new GarmentServiceSubconCuttingSize(new Guid(), new SizeId(1), "", 1, new UomId(1), "", "", ServiceSubconCuttingItemGuid, new Guid(), new Guid(), new ProductId(1), "", "")
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
            _mockServiceSubconCuttingSizeRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconCuttingSize>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconCuttingSize>()));

			//_mockGarmentPreparingRepository
			//    .Setup(s => s.RoChecking(It.IsAny<IEnumerable<string>>()))
			//    .Returns(true);

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