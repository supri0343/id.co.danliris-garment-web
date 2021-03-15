using Barebone.Tests;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Application.GarmentSubcon.GarmentSubconDeliveryLetterOuts.CommandHandlers;
using Xunit;
using System.Threading.Tasks;
using System.Threading;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Commands;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.ReadModels;
using System.Linq;
using System.Linq.Expressions;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts;
using FluentAssertions;

namespace Manufactures.Tests.CommandHandlers.GarmentSubcon.GarmentSubconDeliveryLetterOuts
{
    public class UpdateGarmentSubconDeliveryLetterOutCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSubconDeliveryLetterOutRepository> _mockSubconDeliveryLetterOutRepository;
        private readonly Mock<IGarmentSubconDeliveryLetterOutItemRepository> _mockSubconDeliveryLetterOutItemRepository;

        public UpdateGarmentSubconDeliveryLetterOutCommandHandlerTests()
        {
            _mockSubconDeliveryLetterOutRepository = CreateMock<IGarmentSubconDeliveryLetterOutRepository>();
            _mockSubconDeliveryLetterOutItemRepository = CreateMock<IGarmentSubconDeliveryLetterOutItemRepository>();

            _MockStorage.SetupStorage(_mockSubconDeliveryLetterOutRepository);
            _MockStorage.SetupStorage(_mockSubconDeliveryLetterOutItemRepository);
        }

        private UpdateGarmentSubconDeliveryLetterOutCommandHandler CreateUpdateGarmentSubconDeliveryLetterOutCommandHandler()
        {
            return new UpdateGarmentSubconDeliveryLetterOutCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_BB()
        {
            // Arrange
            Guid SubconDeliveryLetterOutGuid = Guid.NewGuid();
            Guid sewingInId = Guid.NewGuid();
            UpdateGarmentSubconDeliveryLetterOutCommandHandler unitUnderTest = CreateUpdateGarmentSubconDeliveryLetterOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateGarmentSubconDeliveryLetterOutCommand UpdateGarmentSubconDeliveryLetterOutCommand = new UpdateGarmentSubconDeliveryLetterOutCommand()
            {
                ContractNo = "test",
                ContractType = "SUBCON BAHAN BAKU",
                DLDate = DateTimeOffset.Now,
                DLType = "PROSES",
                EPOItemId = 1,
                IsUsed = false,
                PONo = "test",
                Remark = "test",
                TotalQty = 10,
                UsedQty = 10,
                SubconContractId = new Guid(),
                UENId = 1,
                UENNo = "test",
                Items = new List<GarmentSubconDeliveryLetterOutItemValueObject>
                {
                    new GarmentSubconDeliveryLetterOutItemValueObject
                    {
                        Product = new Product(1, "ProductCode", "ProductName"),
                        Quantity=1,
                        DesignColor= "ColorD",
                        SubconDeliveryLetterOutId=Guid.NewGuid(),
                        FabricType="test",
                        ProductRemark="test",
                        UENItemId=1,
                        Uom=new Uom(1,"UomUnit"),
                        UomOut=new Uom(1,"UomUnit"),
                        ContractQuantity=1
                    }
                },
            };
            UpdateGarmentSubconDeliveryLetterOutCommand.SetIdentity(SubconDeliveryLetterOutGuid);

            _mockSubconDeliveryLetterOutRepository
               .Setup(s => s.Query)
               .Returns(new List<GarmentSubconDeliveryLetterOutReadModel>()
               {
                    new GarmentSubconDeliveryLetterOutReadModel(SubconDeliveryLetterOutGuid)
               }.AsQueryable());
            _mockSubconDeliveryLetterOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconDeliveryLetterOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentSubconDeliveryLetterOutItem>()
                {
                    new GarmentSubconDeliveryLetterOutItem(Guid.Empty,SubconDeliveryLetterOutGuid,1,new ProductId(1),"code","name","remark","color",1,new UomId(1),"unit",new UomId(1),"unit","fabType",new Guid(),"","","")
                });

            _mockSubconDeliveryLetterOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconDeliveryLetterOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconDeliveryLetterOut>()));
            _mockSubconDeliveryLetterOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconDeliveryLetterOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconDeliveryLetterOutItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(UpdateGarmentSubconDeliveryLetterOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_CT()
        {
            // Arrange
            Guid SubconDeliveryLetterOutGuid = Guid.NewGuid();
            Guid sewingInId = Guid.NewGuid();
            UpdateGarmentSubconDeliveryLetterOutCommandHandler unitUnderTest = CreateUpdateGarmentSubconDeliveryLetterOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateGarmentSubconDeliveryLetterOutCommand UpdateGarmentSubconDeliveryLetterOutCommand = new UpdateGarmentSubconDeliveryLetterOutCommand()
            {
                ContractNo = "test",
                ContractType = "SUBCON CUTTING",
                DLDate = DateTimeOffset.Now,
                DLType = "RE PROSES",
                EPOItemId = 1,
                IsUsed = false,
                PONo = "test",
                Remark = "test",
                TotalQty = 10,
                UsedQty = 10,
                SubconContractId = new Guid(),
                UENId = 1,
                UENNo = "test",
                Items = new List<GarmentSubconDeliveryLetterOutItemValueObject>
                {
                    new GarmentSubconDeliveryLetterOutItemValueObject
                    {
                        Product = new Product(1, "ProductCode", "ProductName"),
                        Quantity=1,
                        DesignColor= "ColorD",
                        SubconDeliveryLetterOutId=Guid.NewGuid(),
                        FabricType="test",
                        ProductRemark="test",
                        UENItemId=1,
                        Uom=new Uom(1,"UomUnit"),
                        UomOut=new Uom(1,"UomUnit"),
                        ContractQuantity=1
                    }
                },
            };
            UpdateGarmentSubconDeliveryLetterOutCommand.SetIdentity(SubconDeliveryLetterOutGuid);

            _mockSubconDeliveryLetterOutRepository
               .Setup(s => s.Query)
               .Returns(new List<GarmentSubconDeliveryLetterOutReadModel>()
               {
                    new GarmentSubconDeliveryLetterOutReadModel(SubconDeliveryLetterOutGuid)
               }.AsQueryable());
            _mockSubconDeliveryLetterOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconDeliveryLetterOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentSubconDeliveryLetterOutItem>()
                {
                    new GarmentSubconDeliveryLetterOutItem(Guid.Empty,SubconDeliveryLetterOutGuid,1,new ProductId(1),"code","name","remark","color",1,new UomId(1),"unit",new UomId(1),"unit","fabType",new Guid(),"","","")
                });

            _mockSubconDeliveryLetterOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconDeliveryLetterOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconDeliveryLetterOut>()));
            _mockSubconDeliveryLetterOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconDeliveryLetterOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconDeliveryLetterOutItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(UpdateGarmentSubconDeliveryLetterOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_JS()
        {
            // Arrange
            Guid SubconDeliveryLetterOutGuid = Guid.NewGuid();
            Guid sewingInId = Guid.NewGuid();
            UpdateGarmentSubconDeliveryLetterOutCommandHandler unitUnderTest = CreateUpdateGarmentSubconDeliveryLetterOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateGarmentSubconDeliveryLetterOutCommand UpdateGarmentSubconDeliveryLetterOutCommand = new UpdateGarmentSubconDeliveryLetterOutCommand()
            {
                ContractNo = "test",
                ContractType = "SUBCON JASA",
                DLDate = DateTimeOffset.Now,
                DLType = "RE PROSES",
                EPOItemId = 1,
                IsUsed = false,
                PONo = "test",
                Remark = "test",
                TotalQty = 10,
                UsedQty = 10,
                SubconContractId = new Guid(),
                UENId = 1,
                UENNo = "test",
                Items = new List<GarmentSubconDeliveryLetterOutItemValueObject>
                {
                    new GarmentSubconDeliveryLetterOutItemValueObject
                    {
                        Product = new Product(1, "ProductCode", "ProductName"),
                        Quantity=1,
                        DesignColor= "ColorD",
                        SubconDeliveryLetterOutId=Guid.NewGuid(),
                        FabricType="test",
                        ProductRemark="test",
                        UENItemId=1,
                        Uom=new Uom(1,"UomUnit"),
                        UomOut=new Uom(1,"UomUnit"),
                        ContractQuantity=1
                    }
                },
            };
            UpdateGarmentSubconDeliveryLetterOutCommand.SetIdentity(SubconDeliveryLetterOutGuid);

            _mockSubconDeliveryLetterOutRepository
               .Setup(s => s.Query)
               .Returns(new List<GarmentSubconDeliveryLetterOutReadModel>()
               {
                    new GarmentSubconDeliveryLetterOutReadModel(SubconDeliveryLetterOutGuid)
               }.AsQueryable());
            _mockSubconDeliveryLetterOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconDeliveryLetterOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentSubconDeliveryLetterOutItem>()
                {
                    new GarmentSubconDeliveryLetterOutItem(Guid.Empty,SubconDeliveryLetterOutGuid,1,new ProductId(1),"code","name","remark","color",1,new UomId(1),"unit",new UomId(1),"unit","fabType",new Guid(),"","","")
                });

            _mockSubconDeliveryLetterOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconDeliveryLetterOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconDeliveryLetterOut>()));
            _mockSubconDeliveryLetterOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconDeliveryLetterOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconDeliveryLetterOutItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(UpdateGarmentSubconDeliveryLetterOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
