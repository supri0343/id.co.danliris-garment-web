using Barebone.Tests;
using Manufactures.Application.GarmentSample.GarmentServiceSampleCuttings.CommandHandlers;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings.ValueObjects;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings.ReadModels;
using System.Linq.Expressions;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings;
using System.Linq;
using FluentAssertions;
using Manufactures.Domain.GarmentPreparings.Repositories;

namespace Manufactures.Tests.CommandHandlers.GarmentSample.GarmentServiceSampleCuttings
{
    public class UpdateGarmentServiceSampleCuttingCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentServiceSampleCuttingRepository> _mockServiceSampleCuttingRepository;
        private readonly Mock<IGarmentServiceSampleCuttingItemRepository> _mockServiceSampleCuttingItemRepository;
        private readonly Mock<IGarmentServiceSampleCuttingDetailRepository> _mockServiceSampleCuttingDetailRepository;
        private readonly Mock<IGarmentServiceSampleCuttingSizeRepository> _mockServiceSampleCuttingSizeRepository;
        //private readonly Mock<IGarmentPreparingRepository> _mockGarmentPreparingRepository;

        public UpdateGarmentServiceSampleCuttingCommandHandlerTests()
        {
            _mockServiceSampleCuttingRepository = CreateMock<IGarmentServiceSampleCuttingRepository>();
            _mockServiceSampleCuttingItemRepository = CreateMock<IGarmentServiceSampleCuttingItemRepository>();
            _mockServiceSampleCuttingDetailRepository = CreateMock<IGarmentServiceSampleCuttingDetailRepository>();
            _mockServiceSampleCuttingSizeRepository = CreateMock<IGarmentServiceSampleCuttingSizeRepository>();
            //_mockGarmentPreparingRepository = CreateMock<IGarmentPreparingRepository>();

            _MockStorage.SetupStorage(_mockServiceSampleCuttingRepository);
            _MockStorage.SetupStorage(_mockServiceSampleCuttingItemRepository);
            _MockStorage.SetupStorage(_mockServiceSampleCuttingDetailRepository);
            _MockStorage.SetupStorage(_mockServiceSampleCuttingSizeRepository);
            //_MockStorage.SetupStorage(_mockGarmentPreparingRepository);
        }
        private UpdateGarmentServiceSampleCuttingCommandHandler CreateUpdateGarmentServiceSampleCuttingCommandHandler()
        {
            return new UpdateGarmentServiceSampleCuttingCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid ServiceSampleCuttingGuid = Guid.NewGuid();
            Guid ServiceSampleCuttingItemGuid = Guid.NewGuid();
            UpdateGarmentServiceSampleCuttingCommandHandler unitUnderTest = CreateUpdateGarmentServiceSampleCuttingCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateGarmentServiceSampleCuttingCommand UpdateGarmentServiceSampleCuttingCommand = new UpdateGarmentServiceSampleCuttingCommand()
            {
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                SampleDate = DateTimeOffset.Now,
                SampleType = "PRINT",
                Buyer = new Buyer(1, "BuyerCode", "BuyerName"),
                Uom = new Uom(1, "UomUnit"),
                Items = new List<GarmentServiceSampleCuttingItemValueObject>
                {
                    new GarmentServiceSampleCuttingItemValueObject
                    {

                        Article = "Article",
                        Comodity = new GarmentComodity(1, "ComoCode", "ComoName"),
                        RONo = "RONo",
                        ServiceSampleCuttingId=Guid.NewGuid(),
                        Details= new List<GarmentServiceSampleCuttingDetailValueObject>
                        {
                            new GarmentServiceSampleCuttingDetailValueObject
                            {
                                IsSave=true,
                                Quantity=1,
                                DesignColor= "ColorD",
                                CuttingInQuantity=1,
                                //CuttingInDetailId=Guid.NewGuid(),
                                Sizes = new List<GarmentServiceSampleCuttingSizeValueObject>
                                {
                                    new GarmentServiceSampleCuttingSizeValueObject
                                    {
                                        CuttingInId = Guid.NewGuid(),
                                        CuttingInDetailId = Guid.NewGuid(),
                                        Id = Guid.NewGuid(),
                                        ServiceSampleCuttingDetailId = Guid.NewGuid(),
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
            UpdateGarmentServiceSampleCuttingCommand.SetIdentity(ServiceSampleCuttingGuid);

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
                    new GarmentServiceSampleCuttingDetail(new Guid(), ServiceSampleCuttingItemGuid, "ColorD", 1)
                });
            _mockServiceSampleCuttingSizeRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSampleCuttingSizeReadModel, bool>>>()))
                .Returns(new List<GarmentServiceSampleCuttingSize>()
                {
                    new GarmentServiceSampleCuttingSize(new Guid(), new SizeId(1), "", 1, new UomId(1), "", "", ServiceSampleCuttingItemGuid, new Guid(), new Guid(), new ProductId(1), "", "")
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

			//_mockGarmentPreparingRepository
			//    .Setup(s => s.RoChecking(It.IsAny<IEnumerable<string>>()))
			//    .Returns(true);

			_MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(UpdateGarmentServiceSampleCuttingCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}