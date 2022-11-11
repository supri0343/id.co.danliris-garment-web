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
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSubcon.SubconReprocess
{
    public class PlaceGarmentSubconReprocessCommandHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSubconReprocessRepository> _mockSubconReprocessRepository;
        private readonly Mock<IGarmentSubconReprocessItemRepository> _mockSubconReprocessItemRepository;
        private readonly Mock<IGarmentSubconReprocessDetailRepository> _mockGarmentSubconReprocessDetailRepository;

        public PlaceGarmentSubconReprocessCommandHandlerTest()
        {
            _mockSubconReprocessRepository = CreateMock<IGarmentSubconReprocessRepository>();
            _mockSubconReprocessItemRepository = CreateMock<IGarmentSubconReprocessItemRepository>();
            _mockGarmentSubconReprocessDetailRepository = CreateMock<IGarmentSubconReprocessDetailRepository>();

            _MockStorage.SetupStorage(_mockSubconReprocessRepository);
            _MockStorage.SetupStorage(_mockSubconReprocessItemRepository);
            _MockStorage.SetupStorage(_mockGarmentSubconReprocessDetailRepository);
        }
        private PlaceGarmentSubconReprocessCommandHandler CreatePlaceGarmentSubconReprocessCommandHandler()
        {
            return new PlaceGarmentSubconReprocessCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid SubconCustomsOutGuid = Guid.NewGuid();
            Guid SubconDLOutGuid = Guid.NewGuid();
            PlaceGarmentSubconReprocessCommandHandler unitUnderTest = CreatePlaceGarmentSubconReprocessCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentSubconReprocessCommand placeGarmentSubconReprocessCommand = new PlaceGarmentSubconReprocessCommand()
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
            _mockSubconReprocessRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconReprocess>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconReprocess>()));
            _mockSubconReprocessRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSubconReprocessReadModel>().AsQueryable());
            _mockSubconReprocessItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconReprocessItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconReprocessItem>()));

            _mockGarmentSubconReprocessDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconReprocessDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconReprocessDetail>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(placeGarmentSubconReprocessCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
