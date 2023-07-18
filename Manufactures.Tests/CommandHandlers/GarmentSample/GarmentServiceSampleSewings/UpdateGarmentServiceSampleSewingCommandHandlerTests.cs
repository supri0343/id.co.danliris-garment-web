using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.GarmentServiceSampleSewings.CommandHandlers;
using Manufactures.Domain.GarmentPreparings.Repositories;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.ReadModels;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.Repositories;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSample.GarmentServiceSampleSewings
{
    public class UpdateGarmentServiceSampleSewingCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentServiceSampleSewingRepository> _mockServiceSampleSewingRepository;
        private readonly Mock<IGarmentServiceSampleSewingItemRepository> _mockServiceSampleSewingItemRepository;
		private readonly Mock<IGarmentServiceSampleSewingDetailRepository> _mockServiceSampleSewingDetailRepository;
		//private readonly Mock<IGarmentPreparingRepository> _mockGarmentPreparingRepository;

		public UpdateGarmentServiceSampleSewingCommandHandlerTests()
        {
            _mockServiceSampleSewingRepository = CreateMock<IGarmentServiceSampleSewingRepository>();
            _mockServiceSampleSewingItemRepository = CreateMock<IGarmentServiceSampleSewingItemRepository>();
			_mockServiceSampleSewingDetailRepository = CreateMock<IGarmentServiceSampleSewingDetailRepository>();
			//_mockGarmentPreparingRepository = CreateMock<IGarmentPreparingRepository>();

			_MockStorage.SetupStorage(_mockServiceSampleSewingRepository);
            _MockStorage.SetupStorage(_mockServiceSampleSewingItemRepository);
			//_MockStorage.SetupStorage(_mockGarmentPreparingRepository);
			_MockStorage.SetupStorage(_mockServiceSampleSewingDetailRepository);
		}

		private UpdateGarmentServiceSampleSewingCommandHandler CreateUpdateGarmentServiceSampleSewingCommandHandler()
		{
			return new UpdateGarmentServiceSampleSewingCommandHandler(_MockStorage.Object);
		}

		[Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid sewingInItemGuid = Guid.NewGuid();
            Guid serviceSampleSewingGuid = Guid.NewGuid();
            Guid sewingInId = Guid.NewGuid();
            Guid serviceSampleSewingItemGuid = Guid.NewGuid();
            UpdateGarmentServiceSampleSewingCommandHandler unitUnderTest = CreateUpdateGarmentServiceSampleSewingCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateGarmentServiceSampleSewingCommand UpdateGarmentServiceSampleSewingCommand = new UpdateGarmentServiceSampleSewingCommand()
            {
                //Buyer = new Buyer(1, "BuyerCode", "BuyerName"),
                //Items = new List<GarmentServiceSampleSewingItemValueObject>
                //{
                //    new GarmentServiceSampleSewingItemValueObject
                //    {
                //        RONo = "RONo",
                //        Article = "Article",
                //        Comodity = new GarmentComodity(1, "ComoCode", "ComoName"),
                //        Buyer = new Buyer(1, "BuyerCode", "BuyerName"),
                //        Details= new List<GarmentServiceSampleSewingDetailValueObject>
                //        {
                //            new GarmentServiceSampleSewingDetailValueObject
                //            {
                //                Product = new Product(1, "ProductCode", "ProductName"),
                //                Uom = new Uom(1, "UomUnit"),
                //                SewingInId= new Guid(),
                //                SewingInItemId=sewingInItemGuid,
                //                IsSave=true,
                //                Quantity=1,
                //                DesignColor= "ColorD",
                //                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                //            }
                //        }

                //    }
                //},
                ServiceSampleSewingNo = "serviceSewingNo",
                Buyer = new Buyer(1, "buyerCode", "buyerName"),
                ServiceSampleSewingDate = DateTimeOffset.Now,
                QtyPacking = 1,
                UomUnit = "uomUnit",
                NettWeight = 1,
                GrossWeight = 1,
                Items = new List<GarmentServiceSampleSewingItemValueObject>
				{
                    new GarmentServiceSampleSewingItemValueObject
					{
                        RONo = "rONo",
                        Article = "article",
                        Comodity = new GarmentComodity(1, "comodityCode", "comodityName"),
                        Buyer = new Buyer(1, "buyerCode", "buyerName"),
                        Unit = new UnitDepartment(1, "unitCode", "unitName"),
                        Details = new List<GarmentServiceSampleSewingDetailValueObject>
						{
                            new GarmentServiceSampleSewingDetailValueObject
							{
                                Product = new Product(1, "productCode", "productName"),
                                Unit = new UnitDepartment(1, "unitCode", "unitName"),
                                DesignColor = "designColor",
                                Quantity = 1,
                                Uom = new Uom(1, "uomUnit"),
                                IsSave = true,
                                SewingInQuantity = 1,
                                TotalQuantity = 1,
                                Remark = "remark",
                                Color = "1"
							}
						}
					}
				}
            };

            UpdateGarmentServiceSampleSewingCommand.SetIdentity(serviceSampleSewingGuid);

            //_mockServiceSampleSewingRepository
            //    .Setup(s => s.Query)
            //    .Returns(new List<GarmentServiceSampleSewingReadModel>()
            //    {
            //        new GarmentServiceSampleSewingReadModel(serviceSampleSewingGuid)
            //    }.AsQueryable());

			_mockServiceSampleSewingItemRepository
				.Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSampleSewingItemReadModel, bool>>>()))
				.Returns(new List<GarmentServiceSampleSewingItem>()
				{
					new GarmentServiceSampleSewingItem(
						Guid.Empty,
						serviceSampleSewingGuid,
						null,
						null,
						new GarmentComodityId(1),
						null,
						null,
						new BuyerId(1),
						null,
						null,
                        new UnitDepartmentId(1),
                        null,
                        null)
				});
			_mockServiceSampleSewingDetailRepository
				.Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSampleSewingDetailReadModel, bool>>>()))
				.Returns(new List<GarmentServiceSampleSewingDetail>()
				{
					new GarmentServiceSampleSewingDetail(
						new Guid(),
						serviceSampleSewingItemGuid,
						Guid.Empty,
						Guid.Empty,
						new ProductId(1),
						null,
						null,
						null,
						1,
						new UomId(1),
						null,
						new UnitDepartmentId(1),
						null,
						null,
                        null,
                        null)
				});

            _mockServiceSampleSewingRepository
              .Setup(s => s.Query)
              .Returns(new List<GarmentServiceSampleSewingReadModel>()
              {
                    new GarmentServiceSampleSewing(serviceSampleSewingGuid, "serviceSampleSewingNo", DateTimeOffset.Now, true, new BuyerId(1), "buyerCode", "buyerName", 1, "uomUnit", 1, 1).GetReadModel()
              }.AsQueryable());

   //         _mockServiceSampleSewingItemRepository
   //           .Setup(s => s.Query)
   //           .Returns(new List<GarmentServiceSampleSewingItemReadModel>()
   //           {
   //                 new GarmentServiceSampleSewingItem(serviceSampleSewingItemGuid, serviceSampleSewingGuid, "rONo", "article",  new GarmentComodityId(1), "comodityCode", "comodityName", new BuyerId(1), "buyerCode", "buyerName", new UnitDepartmentId(1), "unitCode", "unitName").GetReadModel()
   //           }.AsQueryable());

			//_mockServiceSampleSewingDetailRepository
			//  .Setup(s => s.Query)
			//  .Returns(new List<GarmentServiceSampleSewingDetailReadModel>()
			//  {
			//		new GarmentServiceSampleSewingDetail(new Guid(), serviceSampleSewingItemGuid, new Guid(), new Guid(), new ProductId(1), "productCode", "productName", "designColor", 1, new UomId(1), "uomUnit", new UnitDepartmentId(1), "unitCode", "unitName", "remark", "color").GetReadModel()
			//  }.AsQueryable());

			_mockServiceSampleSewingRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSampleSewing>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSampleSewing>()));

			_mockServiceSampleSewingItemRepository
				.Setup(s => s.Update(It.IsAny<GarmentServiceSampleSewingItem>()))
				.Returns(Task.FromResult(It.IsAny<GarmentServiceSampleSewingItem>()));
			_mockServiceSampleSewingDetailRepository
				.Setup(s => s.Update(It.IsAny<GarmentServiceSampleSewingDetail>()))
				.Returns(Task.FromResult(It.IsAny<GarmentServiceSampleSewingDetail>()));

			//_mockGarmentPreparingRepository
			//	.Setup(s => s.RoChecking(It.IsAny<IEnumerable<string>>()))
			//	.Returns(true);

			_MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(UpdateGarmentServiceSampleSewingCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
