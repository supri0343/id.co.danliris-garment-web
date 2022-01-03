using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.SampleSewingOuts.CommandHandlers;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.ReadModels;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingIns;
using Manufactures.Domain.GarmentSample.SampleSewingIns.ReadModels;
using Manufactures.Domain.GarmentSample.SampleSewingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingOuts;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.ReadModels;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.ValueObjects;
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

namespace Manufactures.Tests.CommandHandlers.GarmentSample.SampleSewingOuts
{
    public class UpdateGarmentSampleSewingOutCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSampleSewingOutRepository> _mockSewingOutRepository;
        private readonly Mock<IGarmentSampleSewingOutItemRepository> _mockSewingOutItemRepository;
        private readonly Mock<IGarmentSampleSewingOutDetailRepository> _mockSewingOutDetailRepository;
        private readonly Mock<IGarmentSampleSewingInItemRepository> _mockSewingInItemRepository;
        private readonly Mock<IGarmentSampleCuttingInRepository> _mockCuttingInRepository;
        private readonly Mock<IGarmentSampleCuttingInItemRepository> _mockCuttingInItemRepository;
        private readonly Mock<IGarmentSampleCuttingInDetailRepository> _mockCuttingInDetailRepository;
        private readonly Mock<IGarmentComodityPriceRepository> _mockComodityPriceRepository;

        public UpdateGarmentSampleSewingOutCommandHandlerTests()
        {
            _mockSewingOutRepository = CreateMock<IGarmentSampleSewingOutRepository>();
            _mockSewingOutItemRepository = CreateMock<IGarmentSampleSewingOutItemRepository>();
            _mockSewingOutDetailRepository = CreateMock<IGarmentSampleSewingOutDetailRepository>();
            _mockSewingInItemRepository = CreateMock<IGarmentSampleSewingInItemRepository>();
            _mockCuttingInRepository = CreateMock<IGarmentSampleCuttingInRepository>();
            _mockCuttingInItemRepository = CreateMock<IGarmentSampleCuttingInItemRepository>();
            _mockCuttingInDetailRepository = CreateMock<IGarmentSampleCuttingInDetailRepository>();
            _mockComodityPriceRepository = CreateMock<IGarmentComodityPriceRepository>();

            _MockStorage.SetupStorage(_mockSewingOutRepository);
            _MockStorage.SetupStorage(_mockSewingOutItemRepository);
            _MockStorage.SetupStorage(_mockSewingOutDetailRepository);
            _MockStorage.SetupStorage(_mockSewingInItemRepository);
            _MockStorage.SetupStorage(_mockCuttingInRepository);
            _MockStorage.SetupStorage(_mockCuttingInItemRepository);
            _MockStorage.SetupStorage(_mockCuttingInDetailRepository);
            _MockStorage.SetupStorage(_mockComodityPriceRepository);
        }
        private UpdateGarmentSampleSewingOutCommandHandler CreateUpdateGarmentSampleSewingOutCommandHandler()
        {
            return new UpdateGarmentSampleSewingOutCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid sewingInItemGuid = Guid.NewGuid();
            Guid sewingOutGuid = Guid.NewGuid();
            Guid sewingInId = Guid.NewGuid();
            UpdateGarmentSampleSewingOutCommandHandler unitUnderTest = CreateUpdateGarmentSampleSewingOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            Manufactures.Domain.GarmentSample.SampleSewingOuts.Commands.UpdateGarmentSampleSewingOutCommand UpdateGarmentSampleSewingOutCommand = new Manufactures.Domain.GarmentSample.SampleSewingOuts.Commands.UpdateGarmentSampleSewingOutCommand()
            {
                RONo = "RONo",
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                UnitTo = new UnitDepartment(2, "UnitCode2", "UnitName2"),
                Article = "Article",
                IsDifferentSize = true,
                Buyer = new Buyer(1, "BuyerCode", "BuyerName"),
                SewingTo = "FINISHING",
                Comodity = new GarmentComodity(1, "ComoCode", "ComoName"),
                SewingOutDate = DateTimeOffset.Now,
                Items = new List<GarmentSampleSewingOutItemValueObject>
                {
                    new GarmentSampleSewingOutItemValueObject
                    {
                        Product = new Product(1, "ProductCode", "ProductName"),
                        Uom = new Uom(1, "UomUnit"),
                        SewingInId= sewingInId,
                        SewingInItemId=sewingInItemGuid,
                        Color="Color",
                        Size=new SizeValueObject(1, "Size"),
                        IsSave=true,
                        Quantity=1,
                        DesignColor= "ColorD",
                        Details = new List<GarmentSampleSewingOutDetailValueObject>
                        {
                            new GarmentSampleSewingOutDetailValueObject
                            {
                                Size=new SizeValueObject(1, "Size"),
                                Uom = new Uom(1, "UomUnit"),
                                Quantity=1
                            }
                        }
                    }
                },

            };
            UpdateGarmentSampleSewingOutCommand.SetIdentity(sewingOutGuid);

            _mockSewingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleSewingOutReadModel>()
                {
                    new GarmentSampleSewingOutReadModel(sewingOutGuid)
                }.AsQueryable());
            _mockSewingOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleSewingOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentSampleSewingOutItem>()
                {
                    new GarmentSampleSewingOutItem(Guid.Empty, sewingOutGuid, Guid.Empty,sewingInItemGuid,new ProductId(1),null,null,null,new SizeId(1), null, 1, new UomId(1), null,null, 1,1,1)
                });
            _mockSewingOutDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleSewingOutDetailReadModel, bool>>>()))
                .Returns(new List<GarmentSampleSewingOutDetail>()
                {
                    new GarmentSampleSewingOutDetail(Guid.Empty, Guid.Empty,new SizeId(1), null, 1, new UomId(1),null )
                });

            GarmentComodityPrice GarmentSampleComodity = new GarmentComodityPrice(
                Guid.NewGuid(),
                true,
                DateTimeOffset.Now,
                new UnitDepartmentId(UpdateGarmentSampleSewingOutCommand.Unit.Id),
                UpdateGarmentSampleSewingOutCommand.Unit.Code,
                UpdateGarmentSampleSewingOutCommand.Unit.Name,
                new GarmentComodityId(UpdateGarmentSampleSewingOutCommand.Comodity.Id),
                UpdateGarmentSampleSewingOutCommand.Comodity.Code,
                UpdateGarmentSampleSewingOutCommand.Comodity.Name,
                1000
                );
            _mockComodityPriceRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentComodityPriceReadModel>
                {
                    GarmentSampleComodity.GetReadModel()
                }.AsQueryable());

            _mockSewingInItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleSewingInItemReadModel>
                {
                    new GarmentSampleSewingInItemReadModel(sewingInItemGuid)
                }.AsQueryable());

            _mockSewingOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleSewingOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleSewingOut>()));
            _mockSewingOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleSewingOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleSewingOutItem>()));
            _mockSewingOutDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleSewingOutDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleSewingOutDetail>()));
            _mockSewingInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleSewingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleSewingInItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(UpdateGarmentSampleSewingOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
