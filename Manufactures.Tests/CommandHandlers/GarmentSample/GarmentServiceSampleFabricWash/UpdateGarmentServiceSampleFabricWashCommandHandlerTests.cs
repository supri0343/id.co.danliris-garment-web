using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.GarmentServiceSampleFabricWashes.CommandHandlers;
using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes;
using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes.ReadModels;
using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes.Repositories;
using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSample.GarmentServiceSampleFabricWashs
{
    public class UpdateGarmentServiceSampleFabricWashCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentServiceSampleFabricWashRepository> _mockServiceSampleFabricWashRepository;
        private readonly Mock<IGarmentServiceSampleFabricWashItemRepository> _mockServiceSampleFabricWashItemRepository;

        public UpdateGarmentServiceSampleFabricWashCommandHandlerTests()
        {
            _mockServiceSampleFabricWashRepository = CreateMock<IGarmentServiceSampleFabricWashRepository>();
            _mockServiceSampleFabricWashItemRepository = CreateMock<IGarmentServiceSampleFabricWashItemRepository>();

            _MockStorage.SetupStorage(_mockServiceSampleFabricWashRepository);
            _MockStorage.SetupStorage(_mockServiceSampleFabricWashItemRepository);
        }

        private UpdateGarmentServiceSampleFabricWashCommandHandler CreateUpdateGarmentServiceSampleFabricWashCommandHandler()
        {
            return new UpdateGarmentServiceSampleFabricWashCommandHandler(_MockStorage.Object, _MockWebApiContext.Object, _MockHttpService.Object );
        }

        //[Fact]
        //public async Task Handle_StateUnderTest_ExpectedBehavior()
        //{
        //    // Arrange
        //    Guid serviceSampleFabricWashGuid = Guid.NewGuid();
        //    Guid serviceSampleFabricWashItemGuid = Guid.NewGuid();
        //    UpdateGarmentServiceSampleFabricWashCommandHandler unitUnderTest = CreateUpdateGarmentServiceSampleFabricWashCommandHandler();
        //    CancellationToken cancellationToken = CancellationToken.None;
        //    UpdateGarmentServiceSampleFabricWashCommand UpdateGarmentServiceSampleFabricWashCommand = new UpdateGarmentServiceSampleFabricWashCommand()
        //    {
        //        Items = new List<GarmentServiceSampleFabricWashItemValueObject>
        //        {
        //            new GarmentServiceSampleFabricWashItemValueObject
        //            {
        //                UnitExpenditureNo = "unitExpenditureNo",
        //                ExpenditureDate = DateTimeOffset.Now,
        //                UnitSender = new UnitSender(1, "UnitSenderCode", "UnitSenderName"),
        //                UnitRequest = new UnitRequest(1, "UnitRequestCode", "UnitRequestName"),
        //                Details = new List<GarmentServiceSampleFabricWashDetailValueObject>
        //                {
        //                    new GarmentServiceSampleFabricWashDetailValueObject
        //                    {
        //                        Product = new Product(1, "ProductCode", "ProductName","roductRemark"),
        //                        Uom = new Uom(1, "UomUnit"),
        //                        IsSave=true,
        //                        Quantity=1,
        //                        DesignColor= "ColorD",
        //                    }
        //                }

        //            }
        //        },

        //    };

        //    UpdateGarmentServiceSampleFabricWashCommand.SetIdentity(serviceSampleFabricWashGuid);

        //    _mockServiceSampleFabricWashRepository
        //        .Setup(s => s.Query)
        //        .Returns(new List<GarmentServiceSampleFabricWashReadModel>()
        //        {
        //            new GarmentServiceSampleFabricWashReadModel(serviceSampleFabricWashGuid)
        //        }.AsQueryable());

        //    _mockServiceSampleFabricWashRepository
        //        .Setup(s => s.Update(It.IsAny<GarmentServiceSampleFabricWash>()))
        //        .Returns(Task.FromResult(It.IsAny<GarmentServiceSampleFabricWash>()));

        //    _MockStorage
        //        .Setup(x => x.Save())
        //        .Verifiable();

        //    // Act
        //    var result = await unitUnderTest.Handle(UpdateGarmentServiceSampleFabricWashCommand, cancellationToken);

        //    // Assert
        //    result.Should().NotBeNull();
        //}
    }
}
