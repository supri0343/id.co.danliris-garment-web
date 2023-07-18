using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.GarmentServiceSampleCuttings.CommandHandlers;
using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GarmentCuttingIns.ReadModels;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentPreparings.Repositories;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings.ReadModels;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings.Repositories;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSample.GarmentServiceSampleCuttings
{
    public class PlaceGarmentServiceSampleCuttingCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentServiceSampleCuttingRepository> _mockServiceSampleCuttingRepository;
        private readonly Mock<IGarmentServiceSampleCuttingItemRepository> _mockServiceSampleCuttingItemRepository;
        private readonly Mock<IGarmentServiceSampleCuttingDetailRepository> _mockServiceSampleCuttingDetailRepository;
        private readonly Mock<IGarmentServiceSampleCuttingSizeRepository> _mockServiceSampleCuttingSizeRepository;
        private readonly Mock<IGarmentCuttingInRepository> _mockCuttingInRepository;
        private readonly Mock<IGarmentCuttingInItemRepository> _mockCuttingInItemRepository;
        private readonly Mock<IGarmentCuttingInDetailRepository> _mockCuttingInDetailRepository;
        private readonly Mock<IGarmentPreparingRepository> _mockGarmentPreparingRepository;

        public PlaceGarmentServiceSampleCuttingCommandHandlerTests()
        {
            _mockServiceSampleCuttingRepository = CreateMock<IGarmentServiceSampleCuttingRepository>();
            _mockServiceSampleCuttingItemRepository = CreateMock<IGarmentServiceSampleCuttingItemRepository>();
            _mockServiceSampleCuttingDetailRepository = CreateMock<IGarmentServiceSampleCuttingDetailRepository>();
            _mockServiceSampleCuttingSizeRepository = CreateMock<IGarmentServiceSampleCuttingSizeRepository>();
            _mockCuttingInRepository = CreateMock<IGarmentCuttingInRepository>();
            _mockCuttingInItemRepository = CreateMock<IGarmentCuttingInItemRepository>();
            _mockCuttingInDetailRepository = CreateMock<IGarmentCuttingInDetailRepository>();
            _mockGarmentPreparingRepository = CreateMock<IGarmentPreparingRepository>();

            _MockStorage.SetupStorage(_mockCuttingInRepository);
            _MockStorage.SetupStorage(_mockCuttingInItemRepository);
            _MockStorage.SetupStorage(_mockCuttingInDetailRepository);
            _MockStorage.SetupStorage(_mockServiceSampleCuttingSizeRepository);
            _MockStorage.SetupStorage(_mockServiceSampleCuttingRepository);
            _MockStorage.SetupStorage(_mockServiceSampleCuttingItemRepository);
            _MockStorage.SetupStorage(_mockServiceSampleCuttingDetailRepository);
            _MockStorage.SetupStorage(_mockGarmentPreparingRepository);
        }
        private PlaceGarmentServiceSampleCuttingCommandHandler CreatePlaceGarmentServiceSampleCuttingCommandHandler()
        {
            return new PlaceGarmentServiceSampleCuttingCommandHandler(_MockStorage.Object);
        }

        /*[Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_BORDIR()
        {
            // Arrange
            Guid cuttingInGuid = Guid.NewGuid();
            Guid cuttingInDetailGuid= Guid.NewGuid();
            Guid cuttingInItemGuid = Guid.NewGuid();
            Guid SampleCuttingDetailGuid = Guid.NewGuid();
            PlaceGarmentServiceSampleCuttingCommandHandler unitUnderTest = CreatePlaceGarmentServiceSampleCuttingCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentServiceSampleCuttingCommand placeGarmentServiceSampleCuttingCommand = new PlaceGarmentServiceSampleCuttingCommand()
            {
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                SampleDate = DateTimeOffset.Now,
                SampleType="BORDIR",
                Buyer = new Buyer(1, "BuyerCode", "BuyerName"),
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
                                Quantity=20,
                                DesignColor= "ColorD",
                                CuttingInQuantity=20,
                                Sizes= new List<GarmentServiceSampleCuttingSizeValueObject>
                                {
                                    new GarmentServiceSampleCuttingSizeValueObject
                                    {
                                        Product = new Product(1, "ProductCode", "ProductName"),
                                        CuttingInDetailId=cuttingInDetailGuid,
                                        CuttingInId=cuttingInGuid,
                                        Size=new SizeValueObject
                                        {
                                            Size="a",
                                            Id=1,
                                        },
                                        Uom=new Uom
                                        {
                                            Unit="a",
                                            Id=1
                                        },
                                        Color="aa",
                                        Quantity=1,
                                        
                                    }
                                }
                            }
                        }
                    }
                },

            };

            GarmentCuttingIn garmentCuttingIn = new GarmentCuttingIn(cuttingInGuid, "", "", "", "RONo", "", new UnitDepartmentId(1), "", "", DateTimeOffset.Now, 1);
            GarmentCuttingInItem garmentCuttingInItem = new GarmentCuttingInItem(cuttingInItemGuid, cuttingInGuid, new Guid(), 1, "", new Guid(), "");
            GarmentCuttingInDetail garmentCuttingInDetail = new GarmentCuttingInDetail(cuttingInDetailGuid, cuttingInItemGuid, new Guid(), new Guid(), new Guid(), new ProductId(1), "", "", "ColorD", "", 1, new UomId(1), "", 10, new UomId(1), "", 10, 1, 1, 1, "");

            GarmentServiceSampleCuttingDetail garmentServiceSampleCuttingDetail = new GarmentServiceSampleCuttingDetail(SampleCuttingDetailGuid, new Guid(),  "ColorD", 1);
            GarmentServiceSampleCuttingSize garmentServiceSampleCuttingSize = new GarmentServiceSampleCuttingSize(new Guid(),new SizeId(1),"",1,new UomId(1),"", "ColorD", SampleCuttingDetailGuid, cuttingInGuid, cuttingInDetailGuid,new ProductId(1), "", "");
            //cuttingInGuid, cuttingInDetailGuid, new ProductId(1), "", "",
            _mockServiceSampleCuttingRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSampleCuttingReadModel>().AsQueryable());
            //_mockServiceSampleCuttingDetailRepository
            //   .Setup(s => s.Query)
            //   .Returns(new List<GarmentServiceSampleCuttingDetailReadModel>
            //   {
            //       garmentServiceSampleCuttingDetail.GetReadModel()
            //   }.AsQueryable()
            //   );
            _mockServiceSampleCuttingSizeRepository
               .Setup(s => s.Query)
               .Returns(new List<GarmentServiceSampleCuttingSizeReadModel>
               {
                   garmentServiceSampleCuttingSize.GetReadModel()
               }.AsQueryable());

            _mockCuttingInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingInReadModel>
                {
                    garmentCuttingIn.GetReadModel()
                }.AsQueryable());
            _mockCuttingInItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingInItemReadModel>
                {
                    garmentCuttingInItem.GetReadModel()
                }.AsQueryable());
            _mockCuttingInDetailRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingInDetailReadModel>
                {
                    garmentCuttingInDetail.GetReadModel(),
                    garmentCuttingInDetail.GetReadModel()
                }.AsQueryable());

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

            _mockGarmentPreparingRepository
                .Setup(s => s.RoChecking(It.IsAny<IEnumerable<string>>(), string.Empty))
                .Returns(true);

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();
            // Act
            var result = await unitUnderTest.Handle(placeGarmentServiceSampleCuttingCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }*/

        /*[Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_PRINT()
        {
            // Arrange
            Guid cuttingInGuid = Guid.NewGuid();
            Guid cuttingInDetailGuid = Guid.NewGuid();
            Guid cuttingInItemGuid = Guid.NewGuid();
            Guid SampleCuttingDetailGuid = Guid.NewGuid();
            PlaceGarmentServiceSampleCuttingCommandHandler unitUnderTest = CreatePlaceGarmentServiceSampleCuttingCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentServiceSampleCuttingCommand placeGarmentServiceSampleCuttingCommand = new PlaceGarmentServiceSampleCuttingCommand()
            {
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                SampleDate = DateTimeOffset.Now,
                SampleType = "PRINT",
                Buyer = new Buyer(1, "BuyerCode", "BuyerName"),
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
                                Quantity=20,
                                DesignColor= "ColorD",
                                CuttingInQuantity=20,
                                Sizes= new List<GarmentServiceSampleCuttingSizeValueObject>
                                {
                                    new GarmentServiceSampleCuttingSizeValueObject
                                    {
                                        Product = new Product(1, "ProductCode", "ProductName"),
                                        CuttingInDetailId=cuttingInDetailGuid,
                                        CuttingInId=cuttingInGuid,
                                        Size=new SizeValueObject
                                        {
                                            Size="a",
                                            Id=1,
                                        },
                                        Uom=new Uom
                                        {
                                            Unit="a",
                                            Id=1
                                        },
                                        Color="aa",
                                        Quantity=1,
                                    }
                                }
                            }
                        }
                    }
                },
            };
            GarmentCuttingIn garmentCuttingIn = new GarmentCuttingIn(cuttingInGuid, "", "", "", "RONo", "", new UnitDepartmentId(1), "", "", DateTimeOffset.Now, 1);
            GarmentCuttingInItem garmentCuttingInItem = new GarmentCuttingInItem(cuttingInItemGuid, cuttingInGuid, new Guid(), 1, "", new Guid(), "");
            GarmentCuttingInDetail garmentCuttingInDetail = new GarmentCuttingInDetail(cuttingInDetailGuid, cuttingInItemGuid, new Guid(), new Guid(), new Guid(), new ProductId(1), "", "", "ColorD", "", 1, new UomId(1), "", 10, new UomId(1), "", 10, 1, 1, 1, "");

            GarmentServiceSampleCuttingDetail garmentServiceSampleCuttingDetail = new GarmentServiceSampleCuttingDetail(SampleCuttingDetailGuid, new Guid(), "ColorD", 1);
            GarmentServiceSampleCuttingSize garmentServiceSampleCuttingSize = new GarmentServiceSampleCuttingSize(new Guid(), new SizeId(1), "", 1, new UomId(1), "", "ColorD", SampleCuttingDetailGuid, cuttingInGuid, cuttingInDetailGuid, new ProductId(1), "", "");
            //cuttingInGuid, cuttingInDetailGuid, new ProductId(1), "", "",
            _mockServiceSampleCuttingRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSampleCuttingReadModel>().AsQueryable());
            //_mockServiceSampleCuttingDetailRepository
            //   .Setup(s => s.Query)
            //   .Returns(new List<GarmentServiceSampleCuttingDetailReadModel>
            //   {
            //       garmentServiceSampleCuttingDetail.GetReadModel()
            //   }.AsQueryable()
            //   );
            _mockServiceSampleCuttingSizeRepository
               .Setup(s => s.Query)
               .Returns(new List<GarmentServiceSampleCuttingSizeReadModel>
               {
                   garmentServiceSampleCuttingSize.GetReadModel()
               }.AsQueryable()
               );

            _mockCuttingInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingInReadModel>
                {
                    garmentCuttingIn.GetReadModel()
                }.AsQueryable());
            _mockCuttingInItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingInItemReadModel>
                {
                    garmentCuttingInItem.GetReadModel()
                }.AsQueryable());
            _mockCuttingInDetailRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingInDetailReadModel>
                {
                    garmentCuttingInDetail.GetReadModel(),
                    garmentCuttingInDetail.GetReadModel()
                }.AsQueryable());

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

            _mockGarmentPreparingRepository
                .Setup(s => s.RoChecking(It.IsAny<IEnumerable<string>>(), string.Empty))
                .Returns(true);

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(placeGarmentServiceSampleCuttingCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }*/

        /*[Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_PLISKET()
        {
            // Arrange
            Guid cuttingInGuid = Guid.NewGuid();
            Guid cuttingInDetailGuid = Guid.NewGuid();
            Guid cuttingInItemGuid = Guid.NewGuid();
            Guid SampleCuttingDetailGuid = Guid.NewGuid();
            PlaceGarmentServiceSampleCuttingCommandHandler unitUnderTest = CreatePlaceGarmentServiceSampleCuttingCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentServiceSampleCuttingCommand placeGarmentServiceSampleCuttingCommand = new PlaceGarmentServiceSampleCuttingCommand()
            {
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                SampleDate = DateTimeOffset.Now,
                SampleType = "PLISKET",
                Buyer = new Buyer(1, "BuyerCode", "BuyerName"),
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
                                //Product = new Product(1, "ProductCode", "ProductName"),
                                IsSave=true,
                                Quantity=1,
                                DesignColor= "ColorD",
                                CuttingInQuantity=1,
                               // CuttingInDetailId=Guid.NewGuid(),
                                Sizes= new List<GarmentServiceSampleCuttingSizeValueObject>
                                {
                                    new GarmentServiceSampleCuttingSizeValueObject
                                    {
                                        Product = new Product(1, "ProductCode", "ProductName"),
                                        CuttingInDetailId=cuttingInDetailGuid,
                                        CuttingInId=cuttingInGuid,
                                        Size=new SizeValueObject
                                        {
                                            Size="a",
                                            Id=1,
                                        },
                                        Uom=new Uom
                                        {
                                            Unit="a",
                                            Id=1
                                        },
                                        Color="aa",
                                        Quantity=1,
                                    }
                                }
                            }
                        }
                    }
                },
            };
            GarmentCuttingIn garmentCuttingIn = new GarmentCuttingIn(cuttingInGuid, "", "", "", "RONo", "", new UnitDepartmentId(1), "", "", DateTimeOffset.Now, 1);
            GarmentCuttingInItem garmentCuttingInItem = new GarmentCuttingInItem(cuttingInItemGuid, cuttingInGuid, new Guid(), 1, "", new Guid(), "");
            GarmentCuttingInDetail garmentCuttingInDetail = new GarmentCuttingInDetail(cuttingInDetailGuid, cuttingInItemGuid, new Guid(), new Guid(), new Guid(), new ProductId(1), "", "", "ColorD", "", 1, new UomId(1), "", 10, new UomId(1), "", 10, 1, 1, 1, "");

            GarmentServiceSampleCuttingDetail garmentServiceSampleCuttingDetail = new GarmentServiceSampleCuttingDetail(SampleCuttingDetailGuid, new Guid(), "ColorD", 1);
            GarmentServiceSampleCuttingSize garmentServiceSampleCuttingSize = new GarmentServiceSampleCuttingSize(new Guid(), new SizeId(1), "", 1, new UomId(1), "", "ColorD", SampleCuttingDetailGuid, cuttingInGuid, cuttingInDetailGuid, new ProductId(1), "", "");
            //cuttingInGuid, cuttingInDetailGuid, new ProductId(1), "", "",
            _mockServiceSampleCuttingRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSampleCuttingReadModel>().AsQueryable());
            //_mockServiceSampleCuttingDetailRepository
            //   .Setup(s => s.Query)
            //   .Returns(new List<GarmentServiceSampleCuttingDetailReadModel>
            //   {
            //       garmentServiceSampleCuttingDetail.GetReadModel()
            //   }.AsQueryable()
            //   );
            _mockServiceSampleCuttingSizeRepository
               .Setup(s => s.Query)
               .Returns(new List<GarmentServiceSampleCuttingSizeReadModel>
               {
                   garmentServiceSampleCuttingSize.GetReadModel()
               }.AsQueryable()
               );

            _mockCuttingInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingInReadModel>
                {
                    garmentCuttingIn.GetReadModel()
                }.AsQueryable());
            _mockCuttingInItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingInItemReadModel>
                {
                    garmentCuttingInItem.GetReadModel()
                }.AsQueryable());
            _mockCuttingInDetailRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingInDetailReadModel>
                {
                    garmentCuttingInDetail.GetReadModel(),
                    garmentCuttingInDetail.GetReadModel()
                }.AsQueryable());

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

            _mockGarmentPreparingRepository
                .Setup(s => s.RoChecking(It.IsAny<IEnumerable<string>>(), string.Empty))
                .Returns(true);

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(placeGarmentServiceSampleCuttingCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }*/
    }
}
