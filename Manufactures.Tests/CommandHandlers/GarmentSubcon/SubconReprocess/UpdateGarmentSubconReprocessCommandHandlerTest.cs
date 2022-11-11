using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSubcon.SubconReprocess.CommandHandlers;
using Manufactures.Domain.GarmentSubcon.SubconReprocess;
using Manufactures.Domain.GarmentSubcon.SubconReprocess.Commands;
using Manufactures.Domain.GarmentSubcon.SubconReprocess.ReadModels;
using Manufactures.Domain.GarmentSubcon.SubconReprocess.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconReprocess.ValueObjects;
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

namespace Manufactures.Tests.CommandHandlers.GarmentSubcon.SubconReprocess
{
    public class UpdateGarmentSubconReprocessCommandHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSubconReprocessRepository> _mockSubconReprocessRepository;
        private readonly Mock<IGarmentSubconReprocessItemRepository> _mockSubconReprocessItemRepository;
        private readonly Mock<IGarmentSubconReprocessDetailRepository> _mockGarmentSubconReprocessDetailRepository;

        public UpdateGarmentSubconReprocessCommandHandlerTest()
        {
            _mockSubconReprocessRepository = CreateMock<IGarmentSubconReprocessRepository>();
            _mockSubconReprocessItemRepository = CreateMock<IGarmentSubconReprocessItemRepository>();
            _mockGarmentSubconReprocessDetailRepository = CreateMock<IGarmentSubconReprocessDetailRepository>();

            _MockStorage.SetupStorage(_mockSubconReprocessRepository);
            _MockStorage.SetupStorage(_mockSubconReprocessItemRepository);
            _MockStorage.SetupStorage(_mockGarmentSubconReprocessDetailRepository);
        }

        private UpdateGarmentSubconReprocessCommandHandler CreateUpdateGarmentSubconReprocessCommandHandler()
        {
            return new UpdateGarmentSubconReprocessCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid SubconReprocessGuid = Guid.NewGuid();
            var SubconReprocessItemGuid = Guid.NewGuid();
            UpdateGarmentSubconReprocessCommandHandler unitUnderTest = CreateUpdateGarmentSubconReprocessCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateGarmentSubconReprocessCommand updateGarmentSubconReprocessCommand = new UpdateGarmentSubconReprocessCommand()
            {
                Date = DateTimeOffset.Now,
                ReprocessNo = "bcNo",
                ReprocessType = "SUBCON JASA KOMPONEN",
                Items = new List<GarmentSubconReprocessItemValueObject>()
                {
                    new GarmentSubconReprocessItemValueObject
                    {
                       ServiceSubconCuttingId= Guid.NewGuid(),
                       ServiceSubconCuttingItemId= Guid.NewGuid(),
                       Buyer = new Buyer
                       {
                            Code = "test",
                            Id = 1,
                            Name = "test"
                       },
                       Comodity= new GarmentComodity
                       {
                            Code = "test",
                            Id = 1,
                            Name = "test"
                       },
                       Article="art",
                       RONo="ro",
                       ServiceSubconCuttingNo="no",
                       Type="SUBCON JASA KOMPONEN",
                       Details= new List<GarmentSubconReprocessDetailValueObject>()
                       {
                           new GarmentSubconReprocessDetailValueObject
                           {
                               ServiceSubconCuttingDetailId=Guid.NewGuid(),
                               ServiceSubconCuttingSizeId=Guid.NewGuid(),
                               Size= new SizeValueObject
                               {
                                   Id=1,
                                   Size="s"
                               },
                               Color="color",
                               DesignColor="DC",
                               Quantity=1,
                               Remark="aa",
                               RemQty=1,
                               ReprocessQuantity=1,
                               Uom= new Uom
                               {
                                   Id=1,
                                   Unit="m"
                               }
                           }
                       }
                    }
                }
            };
            updateGarmentSubconReprocessCommand.SetIdentity(SubconReprocessGuid);

            _mockSubconReprocessRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSubconReprocessReadModel>()
                {
                    new GarmentSubconReprocessReadModel(SubconReprocessGuid)
                }.AsQueryable());

            _mockSubconReprocessItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconReprocessItemReadModel, bool>>>()))
                .Returns(new List<GarmentSubconReprocessItem>()
                {
                    new GarmentSubconReprocessItem(SubconReprocessItemGuid, SubconReprocessGuid, Guid.NewGuid(), "no", Guid.NewGuid(), Guid.NewGuid(), "no", Guid.NewGuid(),"ro","art", new GarmentComodityId(1), null, null, new BuyerId(1), null, null)
                }); ;

            _mockSubconReprocessItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconReprocessItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconReprocessItem>()));

            _mockSubconReprocessRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconReprocess>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconReprocess>()));

            _mockGarmentSubconReprocessDetailRepository
               .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconReprocessDetailReadModel, bool>>>()))
               .Returns(new List<GarmentSubconReprocessDetail>()
               {
                    new GarmentSubconReprocessDetail(SubconReprocessItemGuid, SubconReprocessItemGuid, new SizeId(1),null,1,1, new UomId(1), null, "color",  new Guid(), new Guid(), new Guid(), "ColorD", new UnitDepartmentId(1), null, null, null)
               });

            _mockGarmentSubconReprocessDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconReprocessDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconReprocessDetail>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(updateGarmentSubconReprocessCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
